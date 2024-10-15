using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.AlgorithmResponse;
using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmPreparers;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Builders;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;
using Backand.FrontendEntities.Links;
using Microsoft.IdentityModel.Tokens;

namespace Backand.Services.AlgorithmServices;

/// <summary>
/// Сервис алгоритмов оптимизации.
/// </summary>
public class AlgorithmService
{
	/// <summary>
	/// Подготовитель данных.
	/// </summary>
    private readonly AlgorithmDataPreparer _dataPreparer;

    public AlgorithmService(AlgorithmData data, ApplicationContext applicationContext) =>
	    _dataPreparer = new AlgorithmDataPreparer(data, applicationContext);
    
    /// <summary>
    /// Является ли Build Type правильным.
    /// </summary>
    /// <param name="filter">Фильтр.</param>
    /// <param name="constructionUnit">Единица постройки.</param>
    /// <returns>Истина - правильный, Ложь - не правильный.</returns>
    private static bool IsValidBuildType(Filter filter, ConstructionUnit constructionUnit) =>
	    filter.BuildType == BuildType.NoMatter ||
	    (BuildType)constructionUnit.ConstructionUnitTypeId == filter.BuildType;

    /// <summary>
    /// Необходим ли сборочный цех.
    /// </summary>
    /// <param name="constructionObject">Объект.</param>
    /// <param name="constructionUnit">Единица постройки.</param>
    /// <returns>Истина - необходим, Ложь - нет.</returns>
    private static bool IsAssemblyRequired(DbEntities.ObjectEntity constructionObject, ConstructionUnit constructionUnit) =>
	    !constructionObject.ContainsAssemblyShop &&
	    (BuildType)constructionUnit.ConstructionUnitTypeId == BuildType.Block;

    /// <summary>
    /// Получить ответ алгоритма
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Ответ алгоритма.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<AlgorithmResponse> GetAlgorithmSolve(AlgorithmRequest request, CancellationToken cancellationToken)
    {
		///Список сооружений с фильтрами у каждого
        var constructionOptions = request.ConstructionOptions;
		///Ответ алгоритма
        var result = new List<Order>();
		///Для каждого сооружения отдельно высчитывается Order (список лучших комплексных вариантов)
        foreach (var constructionOption in constructionOptions)
        {
	        var order = new Order();
			///Объект всех фильтров, выбранных для сооружения
	        var filter = constructionOption.Filter;
			///Объект фильтра доставки (внутри список id, выбранных типов доставки на фронте)
	        var filterTransportTypes = filter.TransportTypeIds;
            ///Если объект фильтра доставки был пустой, то он заполняется всеми типами доставки (3 типа: наземный, воздушный, водный)
            if (filterTransportTypes.IsNullOrEmpty())
                filterTransportTypes = await _dataPreparer.GetAllTransportTypeIds(cancellationToken);
	        
	        var constructionId = constructionOption.ConstructionId;
	        ///Тип сооружения (качалка, вышка и тп)
	        var construction = await _dataPreparer.GetConstructionById(constructionId, cancellationToken) ??
	                           throw new NullReferenceException();
	        
			///Объект, на котором стоит сооружение
	        var (constructionObject, constructionTypeId) = (construction.Object ?? throw new NullReferenceException(),
		        construction.ConstructionTypeId);
	        ///Заполнение Order первичной информацией (сооржуением, объектом, месторождением и дочерним обществом)
			order.Construction = new EntityLink { Id = constructionId, Name = construction.ConstructionName };
			order.Object = new EntityLink { Id = construction.ObjectId, Name = construction.Object.Name! };
			order.Mine = new EntityLink { Id = construction.Object.MineId, Name = construction.Object.Mine!.Name };
			order.Subsidiary = new EntityLink { Id = construction.Object.Mine.SubsidiaryId ?? 0, Name = construction.Object.Mine.Subsidiary.Name };

			///Результат: в результате для этого сооружения заполненые первичные организационные данные


            ///Список дистанций от объекта до каждого существующего склада (с инфой о материалах на складе)
            ///filterTransportTypes в функции вообще не используется (мб убрать)
            var objectToStoragesDistanceWithInfo = await
		        _dataPreparer.GetStorageToConstructionDistanceInfoVector(constructionObject, filterTransportTypes, cancellationToken);
            ///Список id всех складов из objectToStoragesDistanceWithInfo
            var storagesIds = _dataPreparer.GetUniqueStorageIds(objectToStoragesDistanceWithInfo);
            ///Список дистанций от объекта до каждого склада из objectToStoragesDistanceWithInfo. Каким образом заполняется таблица дистанций?
            var objectToStoragesDistance =
		        _dataPreparer.GetStorageToConstructionDistanceVector(objectToStoragesDistanceWithInfo);
            ///Список дистанций от каждого склада до каждого элемента "транспорт_парк" (то есть для каждого парка
            ///(в котором есть наземный транспорт)
            ///и сразу для каждого транспорта в парке (почему-то даже для воздушного и водного транспорта))
            ///filterTransportTypes в функции вообще не используется (мб убрать)
            var transports_FleetsToStoragesDistanceWithInfo = await
		        _dataPreparer.GetTransportToStorageDistanceInfo(storagesIds, filter.TransportTypeIds, cancellationToken);
            ///Список элементов "транспорт_парк" (повторяются) из transports_FleetsToStoragesDistanceWithInfo
            var transports_Fleets = transports_FleetsToStoragesDistanceWithInfo
                .Select(pair => pair.Item1)
		        .ToArray();
            ///Список id элементов "транспорт_парк" (не повторяются) из transports_FleetsToStoragesDistanceWithInfo
            var transports_FleetsIds = _dataPreparer.GetTransportsIds(transports_FleetsToStoragesDistanceWithInfo
                .Select(pair => pair.Item1));

            ///Список скорости и коэффициента расчета стоимости доставки для элементов "транспорт_парк" (не повторяются) из transports_Fleets
			///Помечено как ground, хотя туда попадают и транспорты вода-воздух
            var groundTransports_FleetsSpeedAndCoeff = _dataPreparer.GetTransportInfosVector(transports_Fleets);

            ///Матрица.
            ///Строки - transports_FleetsIds (транспорт на конкретном парке транспорта)
            ///Столбцы - storagesIds (склады)
			///Ячейка - расстояние от парка транспорта этого транспорта до склада
            var transportsToStoragesDistanceMatrix =
		        _dataPreparer.GetTransportsToStoragesDistanceMatrix(transports_FleetsToStoragesDistanceWithInfo,
			        storagesIds,
                    transports_FleetsIds);

            ///Результат: сформирована матрица расстояний от каждого транспорта каждого парка транспорта (только те парки, на которых есть наземный транспорт)
			///(почему то используются и вода+воздух)
            ///до каждого склада материалов из бд



            ///Список комплектов материалов (с материалами) из которых можно построить сооружение.
			///(для дальнейшей постройки должен использоваться 1 комплект: либо роосыпной, либо блочный какого-нибудь производителя)
            var constructionMaterialsSets =
		        _dataPreparer.GetMaterialsSetsWithConstructionTypes(constructionTypeId);

            ///Список элементов "транспорт_парк до объекта". Дистанция от парка этого транспорта до объекта. Берется только вода воздух
            ///filterTransportTypes используется (выбираются водный или воздушный или и тот и другой).
            var transports_FleetsToObjectDistanceWithInfo = await _dataPreparer.GetTransportsToObjectDistance(
                transports_FleetsIds,
		        filterTransportTypes,
		        constructionObject.ObjectId,
		        cancellationToken);
            ///Список элеметов "транспорт_парк до объекта"
            var transports_FleetsToObject = transports_FleetsToObjectDistanceWithInfo
                .Select(pair => pair.transport)
		        .Distinct()
		        .ToArray();
            ///Список скорости и коэффициента для элементов "транспорт_парк до объекта"
            var notGroundTransports_FleetsSpeedAndCoeffToObject = _dataPreparer.GetTransportInfosVector(transports_FleetsToObject);
            ///Список id элеметов "транспорт_парк до объекта"
            var transports_FleetsIdsToObject = _dataPreparer.GetTransportsIds(transports_FleetsToObjectDistanceWithInfo
                .Select(pair => pair.transport));


            ///Список дистанций с доп.инфой от складов до элемента "транспорт_парк" водавоздух (по сути до парка 2-го логиста, который повезет до объекта)
            var storagesToNotGroundTransports_FleetsWithInfo = 
		        await _dataPreparer.GetStoragesToTransportFleetsInfos(transports_FleetsIdsToObject,
		        storagesIds,
		        filterTransportTypes,
		        constructionObject.ObjectId,
		        cancellationToken);
            ///Дистанции от каждого элемента "транспорт_парк до объекта" до объекта
            var transportsToObjectDistanceDecimalVector = _dataPreparer.GetDistanceVector(transports_FleetsToObjectDistanceWithInfo);
            ///Матрица дистанций от каждого склада до каждого элемента "транспорт_парк" (по сути до парков 2-го логиста)
            var transportsToObjectDistanceDecimalMatrix = _dataPreparer.GetStorageToTransportDistanceMatrix(storagesToNotGroundTransports_FleetsWithInfo,
		        storagesIds,
                transports_FleetsIdsToObject);

            ///Результат: составлена матрица дистанций от складов до элементов "транспорт_парк" (по сути до парков транспорта 2-го логиста, которые имеют воздушный и водный транспорт)
			


			///Общие результаты предыдущего блока:
			///1. Вытащена из бд организационная информация об объекте, месторождении, дочернем обществе текущего сооружения
			///2. Составлена матрица дистанций от парков логистов (1-й)(именно назменый, но в коде ошибка, используется и другие) до абсолтно всех складов материалов.
			///3 Составлена матрица дистанций от прежде указанных складов до парков логистов (2-й)(именно водные или воздушные или и те и другие).
			///По сути пока просто вытащены данные из бд
			///Смысл в том, что в дальнейшем при необходимости 1-й логист поедет до склада, а потом поедет до 2-го логиста, у которого будет транспорт, чтобы доставить
			///материалы на труднодоступные объекты воздушной или водной доставкой.
			///






            foreach (var constructionMaterialSet in constructionMaterialsSets)
	        {
		        var orderVariant = new OrderVariant();

				//Получен список материалов из комплекта
		        var constructionUnits = constructionMaterialSet.Value;

		        var materialOrderVariants = new List<MaterialOrderVariant>();
		        
		        orderVariant.MaterialOrderVariants = materialOrderVariants;

                orderVariant.ConstructionUnitType = constructionUnits.First().TypeName;
                //проверяет сразу нет ли цеха и блочный ли это комплект. Если цех есть
                orderVariant.IsAssemblyBuildRequired = IsAssemblyRequired(constructionObject, constructionUnits.First());
                //проверяет подходит ли этот комплект отностительно фильтра способа строительства (если был выбран блочный фильтр, но это комплект россыпи,
				//то итерация скипается на следующий комплект)
                if (!IsValidBuildType(filter, constructionUnits.First())) continue;
                //Получен список id материалов из комплекта
                var constructionUnitIds = _dataPreparer.GetConstructionUnitIds(constructionUnits);
                //Получен список элементов "материал_склад" для каждого материала комплекта и каждого склада
                var constructionUnitsFromStorage =
			        _dataPreparer.GetConstructionUnitsFromStorages(constructionUnitIds);
                //Получена матрица наличия склад_материал
                var materialMatrix = _dataPreparer.GetMaterialParamsMatrix(
			        storagesIds,
			        constructionUnitIds,
			        constructionUnitsFromStorage,
			        constructionUnits);

                //Объект всей информации, которая пойдет в алгоритм
                var solverParameters = new AlgorithmSolverParameters(
                    objectToStoragesDistance.TransformDecimalToLongVector(100),
			        transportsToStoragesDistanceMatrix.TransformDecimalToLongMatrix(100),
			        transportsToObjectDistanceDecimalMatrix.TransformDecimalToLongMatrix(100),
			        transportsToObjectDistanceDecimalVector.TransformDecimalToLongVector(100),
                    groundTransports_FleetsSpeedAndCoeff.TransformTransportInfosToLongMatrix(100),
                    notGroundTransports_FleetsSpeedAndCoeffToObject.TransformTransportInfosToLongMatrix(100),
			        materialMatrix.TransformMaterialParamsToLongMatrix(100),
			        filterTransportTypes);

                //Выбор оптимума (время/цена/средний)
                //не увидел где используется фильтр для определения способа строительства. Надо найти
                //solver это объект от Or-tools Google. Используется для оптимизации
                // filter.FilterMethod - время/цена/средний
                var solver = new AlgorithmSolverBuilder(solverParameters)
			        .BuildSolverFromFilterMethod(filter.FilterMethod);
		        
		        var resultInfos = solver.GetResult();
		        
		        foreach (var resultInfo in resultInfos)
		        {
			        var materialIndex = resultInfo.MaterialIndex;
			        var storageIndex = resultInfo.StorageIndex;
			        var nonGroundIndex = resultInfo.NonGroundTransportIndex;
			        var groundIndex = resultInfo.GroundTransportIndex;

			        var buildInfo = new BuildInfo
			        {
				        ConstructionUnitName = constructionUnits[materialIndex].Name,
				        ConstructionUnitType = constructionUnits[materialIndex].TypeName,
				        MeasuringUnit = constructionUnits[materialIndex].MeasureUnitName,
				        Amount = constructionUnits[materialIndex].Amount
			        };

			        var storage = objectToStoragesDistanceWithInfo[storageIndex].Storage;
			        var productionInfo = new ProductionInfo
			        {
				        ManufacturerName = storage.Manufacturer!.Name,
				        StorageName = storage.Name,
				        StorageAddress = storage.Address,
				        PricePerUnit = materialMatrix[storageIndex, materialIndex]!.PricePerUnit,
				        PurchasePrice = materialMatrix[storageIndex, materialIndex]!.PricePerUnit *
				                        (decimal)constructionUnits[materialIndex].Amount
			        };

			        var groundTransportInfo = transports_Fleets[groundIndex];
			        var fleet = groundTransportInfo.TransportFleet;
			        var company = fleet?.Company;
			        var logisticCompany = company?.LogisticCompany;
			        
			        var groundDeliveryDistance = transportsToStoragesDistanceMatrix[groundIndex, storageIndex] + 
				        ((nonGroundIndex != -1) ? transportsToObjectDistanceDecimalMatrix[storageIndex, nonGroundIndex] : objectToStoragesDistance[storageIndex]);

			        var groundLogisticInfo = new LogisticInfo
			        {
				        LogisticCompanyName = (fleet != null && logisticCompany != null)
					        ? logisticCompany.Name
					        : "Неопределено",
				        TransportFleetName = (fleet != null) ? fleet.Name : "Неопределено",
				        TransportFleetAddress = (fleet != null) ? fleet.Address : "Неопределено",
				        TransportName = groundTransportInfo.Transport.Name,
				        TransportTypeName = groundTransportInfo.Transport.TransportMode.TransportType.Name,
				        TransportModeName = groundTransportInfo.Transport.TransportMode.Name,
				        CoefficientTypeName = groundTransportInfo.CoefficientType.Name,
				        CoefficientValue = groundTransportInfo.CoefficientValue,
				        DeliveryDistance = groundDeliveryDistance,
				        AverageSpeed = groundTransportInfo.AverageSpeed,
			        };
			        
			        var materialOrderVariant = new MaterialOrderVariant
			        {
				        BuildInfo = buildInfo,
				        ProductionInfo = productionInfo,
				        LogisticInfos = new List<LogisticInfo>()
			        };
			        
			        materialOrderVariant.LogisticInfos.Add(groundLogisticInfo);

			        if (nonGroundIndex != -1)
			        {
                        var nonGroundTransportsInfos =
                            transports_FleetsToObjectDistanceWithInfo.Select(pair => pair.Item1)
						        .Distinct().ToArray();
				        var nonGroundTransportInfo = nonGroundTransportsInfos[nonGroundIndex];
				        var nonGroundFleet = nonGroundTransportInfo.TransportFleet;
				        var nonGroundLogisticCompany = company?.LogisticCompany;
				        var nonGroundDeliveryDistance = transportsToObjectDistanceDecimalVector[nonGroundIndex];
				        
				        var nonGroundLogisticInfo = new LogisticInfo()
				        {
					        LogisticCompanyName = (nonGroundFleet != null && nonGroundLogisticCompany != null)
						        ? nonGroundLogisticCompany.Name
						        : "Неопределено",
					        TransportFleetName = (nonGroundFleet != null) ? nonGroundFleet.Name : "Неопределено",
					        TransportFleetAddress = (nonGroundFleet != null) ? nonGroundFleet.Address : "Неопределено",
					        TransportName = nonGroundTransportInfo.Transport.Name,
					        TransportTypeName = nonGroundTransportInfo.Transport.TransportMode.TransportType.Name,
					        TransportModeName = nonGroundTransportInfo.Transport.TransportMode.Name,
					        CoefficientTypeName = nonGroundTransportInfo.CoefficientType.Name,
					        CoefficientValue = nonGroundTransportInfo.CoefficientValue,
					        DeliveryDistance = nonGroundDeliveryDistance,
					        AverageSpeed = nonGroundTransportInfo.AverageSpeed,
				        };
				        
				        materialOrderVariant.LogisticInfos.Add(nonGroundLogisticInfo);
			        }

			        materialOrderVariants.Add(materialOrderVariant);
		        }

				orderVariant.TransportTypeName = orderVariant.GetUniqueTransportTypes(materialOrderVariants);

                orderVariant.MaterialOrderVariants = materialOrderVariants;
		        order.Variants.Add(orderVariant);
	        }
            result.Add(order);
        }

        return new AlgorithmResponse() { Orders = result };
    }
}