using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.AlgorithmResponse;
using Backand.FrontendEntities.Requests;
using Backand.Services.AlgorithmServices.AlgorithmPreparers;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Builders;
using Backand.Services.AlgorithmServices.AlgorithmSolvers.Parameters;

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
    private static bool IsAssemblyRequired(Objects constructionObject, ConstructionUnit constructionUnit) =>
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
        var constructionOptions = request.ConstructionOptions;
        var result = new List<Order>();
        foreach (var constructionOption in constructionOptions)
        {
	        var order = new Order();
	        var filter = constructionOption.Filter;
	        var filterTransportTypes = filter.TransportTypeIds;
	        
	        var constructionId = constructionOption.ConstructionId;
	        
	        var construction = await _dataPreparer.GetConstructionById(constructionId, cancellationToken) ??
	                           throw new NullReferenceException();
	        
	        
	        var (constructionObject, constructionTypeId) = (construction.Object ?? throw new NullReferenceException(),
		        construction.ConstructionTypeId);
	        
	        order.ConstructionName = construction.ConstructionName;
	        order.ConstructionId = constructionId;
	        order.LinkId = construction.Link.Id;
	        order.LinkName = construction.Link.Name;
	        order.ObjectsId = construction.ObjectsId;
	        order.ConstructionType = construction.ConstructionType.Name;
	        
	        var storageToConstructionDistancesInfo = await
		        _dataPreparer.GetStorageToConstructionDistanceInfoVector(constructionObject, filterTransportTypes, cancellationToken);
	        
	        var storagesIds = _dataPreparer.GetUniqueStorageIds(storageToConstructionDistancesInfo);
	        
	        var storageToConstructionDistanceVector =
		        _dataPreparer.GetStorageToConstructionDistanceVector(storageToConstructionDistancesInfo);

	        var transportsToStoragesDistanceInfos = await
		        _dataPreparer.GetTransportToStorageDistanceInfo(storagesIds, filter.TransportTypeIds, cancellationToken);

	        var transportsToStorages = transportsToStoragesDistanceInfos
		        .Select(pair => pair.Item1)
		        .ToArray();
	        
	        var transportToStoragesIds = _dataPreparer.GetTransportsIds(transportsToStoragesDistanceInfos
		        .Select(pair => pair.Item1));


	        var groundTransportInfosVector = _dataPreparer.GetTransportInfosVector(transportsToStorages);

	        var transportsToStoragesDistanceMatrix =
		        _dataPreparer.GetTransportsToStoragesDistanceMatrix(transportsToStoragesDistanceInfos,
			        storagesIds,
			        transportToStoragesIds);

	        var constructionMaterialsSets =
		        _dataPreparer.GetMaterialsSetsWithConstructionTypes(constructionTypeId);

	        var transportsToObjectInfos = await _dataPreparer.GetTransportsToObjectsDistance(
		        transportToStoragesIds,
		        filterTransportTypes,
		        constructionObject.ObjectsId,
		        cancellationToken);

	        var transportsToObject = transportsToObjectInfos
		        .Select(pair => pair.transport)
		        .Distinct()
		        .ToArray();
	        
	        var notGroundTransportInfosVector = _dataPreparer.GetTransportInfosVector(transportsToObject);

	        var transportsToObjectIds = _dataPreparer.GetTransportsIds(transportsToObjectInfos
		        .Select(pair => pair.transport));

	        var storagesToNotGroundTransportsInfos = 
		        await _dataPreparer.GetStoragesToTransportFleetsInfos(transportsToObjectIds,
		        storagesIds,
		        filterTransportTypes,
		        constructionObject.ObjectsId,
		        cancellationToken);

	        var transportsToObjectDistanceDecimalVector = _dataPreparer.GetDistanceVector(transportsToObjectInfos);

	        var transportsToObjectDistanceDecimalMatrix = _dataPreparer.GetStorageToTransportDistanceMatrix(storagesToNotGroundTransportsInfos,
		        storagesIds,
		        transportsToObjectIds);
	        
	        foreach (var constructionMaterialSet in constructionMaterialsSets)
	        {
		        var orderVariant = new OrderVariant();

		        var constructionUnits = constructionMaterialSet.Value;

		        var materialOrderVariants = new List<MaterialOrderVariant>();
		        
		        orderVariant.MaterialOrderVariants = materialOrderVariants;

		        
		        orderVariant.IsAssemblyBuildRequired = IsAssemblyRequired(constructionObject, constructionUnits.First());
		        
		        if (!IsValidBuildType(filter, constructionUnits.First())) continue;
		        var constructionUnitIds = _dataPreparer.GetConstructionUnitIds(constructionUnits);
		        
		        var constructionUnitsFromStorage =
			        _dataPreparer.GetConstructionUnitsFromStorages(constructionUnitIds);

		        var materialMatrix = _dataPreparer.GetMaterialParamsMatrix(
			        storagesIds,
			        constructionUnitIds,
			        constructionUnitsFromStorage,
			        constructionUnits);

		        var solverParameters = new AlgorithmSolverParameters(
			        storageToConstructionDistanceVector.TransformDecimalToLongVector(100),
			        transportsToStoragesDistanceMatrix.TransformDecimalToLongMatrix(100),
			        transportsToObjectDistanceDecimalMatrix.TransformDecimalToLongMatrix(100),
			        transportsToObjectDistanceDecimalVector.TransformDecimalToLongVector(100),
			        groundTransportInfosVector.TransformTransportInfosToLongMatrix(100),
			        notGroundTransportInfosVector.TransformTransportInfosToLongMatrix(100),
			        materialMatrix.TransformMaterialParamsToLongMatrix(100),
			        filterTransportTypes);

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

			        var storage = storageToConstructionDistancesInfo[storageIndex].Storage;
			        var productionInfo = new ProductionInfo
			        {
				        ManufacturerName = storage.Manufacturer!.Name,
				        StorageName = storage.Name,
				        StorageAddress = storage.Address,
				        PricePerUnit = materialMatrix[storageIndex, materialIndex]!.PricePerUnit,
				        PurchasePrice = materialMatrix[storageIndex, materialIndex]!.PricePerUnit *
				                        (decimal)constructionUnits[materialIndex].Amount
			        };

			        var groundTransportInfo = transportsToStorages[groundIndex];
			        var fleet = groundTransportInfo.TransportFleet;
			        var company = fleet?.Company;
			        var logisticCompany = company?.LogisticCompany;
			        
			        var groundDeliveryDistance = transportsToStoragesDistanceMatrix[groundIndex, storageIndex] + 
				        ((nonGroundIndex != -1) ? transportsToObjectDistanceDecimalMatrix[storageIndex, nonGroundIndex] : storageToConstructionDistanceVector[storageIndex]);

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
					        transportsToObjectInfos.Select(pair => pair.Item1)
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
		        orderVariant.MaterialOrderVariants = materialOrderVariants;
		        order.Variants.Add(orderVariant);
		        result.Add(order);
	        }
        }

        return new AlgorithmResponse() { Orders = result };
    }
}