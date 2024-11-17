using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;
using Microsoft.EntityFrameworkCore;
using Backand.Services;
using Backand.ManagersClasses.AlgorithmDataManager.TrackGetters;
using Backand.FrontendEntities.AlgorithmResponse;
using Backand.DbEntities.ConstructionSpace;
using Backand.Services.AlgorithmServices;
using static Backand.ManagersClasses.AlgorithmDataManager.InnerDataRequests;
using static Backand.ManagersClasses.AlgorithmDataManager.DataSorting;
using static Backand.ManagersClasses.AlgorithmDataManager.Delivery;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class AlgorithmDataManagers : DbRequests
	{

		static async Task<AlgorithmData> LoadData(ApplicationContext dbContext, DistanceService distanceService)
		{
			List<Construction> constructions = await dbContext.Construction.Include(c => c.Object).ToListAsync();
			List<Storage> storages = await dbContext.Storage.ToListAsync();
			List<TransportFleet> transportFleets = await dbContext.TransportFleet.ToListAsync();

			List<TransportOnFleetWithRegions> transportsOnFleetsAll = await GetTransportsOnFleetsAsync(dbContext);
			List<MaterialSet> materialSets = await dbContext.MaterialSet.ToListAsync();
			List<StorageMaterial> storagesMaterialsAll = await GetStoragesMaterialsAsync(dbContext);

			TransportFleetToObjectTracker transportFleetToObject = new(dbContext, distanceService);
			StorageToObjectTracker storageToObject = new(dbContext, distanceService);
			StorageToTransportFleetTracker storageToFleet = new(dbContext, distanceService);

			List<StorageToObjectDistance> storageToObjectDistances = await storageToObject.GetTrackToEndpoints();
			List<StorageToTransportFleetDistance> storageToTransportFleetDistances = await storageToFleet.GetTrackToEndpoints();
			List<TransportFleetToObjectDistance> transportFleetToObjectDistance = await transportFleetToObject.GetTrackToEndpoints();
			return new AlgorithmData(constructions, storages, transportFleets, transportsOnFleetsAll, materialSets, storagesMaterialsAll, storageToObjectDistances, storageToTransportFleetDistances, transportFleetToObjectDistance);
		}

		internal static async Task CalculateOrderCostTime(HttpContext context, ApplicationContext dbContext, DistanceService distanceService)
		{
			AlgorithmRequest? algorithmRequest = await context.Request.ReadFromJsonAsync<AlgorithmRequest>() ?? throw new ArgumentNullException("Пустое тело запроса!");
			List<ConstructionWithFilters> constructionOptions = algorithmRequest.Data;

			AlgorithmData dataTuple = await LoadData(dbContext, distanceService);
			var (constructions, storages, transportFleets, transportsOnFleetsAll, materialSets, storagesMaterialsAll, storageToObjectDistances, storageToTransportFleetDistance, transportFleetToObjectDistance) = dataTuple;

			AlgorithmResponse response = new();

			foreach (var constructionOption in constructionOptions)
			{
				(ObjectEntity objectToDeliver, int constructionTypeId) = constructions
					.Where(c => c.ConstructionId == constructionOption.ConstructionId)
					.Select(c => (c.Object!, c.ConstructionTypeId))
					.First();

				FillDeliveryVariants(dataTuple, constructionOption, objectToDeliver);

				List<StorageMaterial> storagesMaterials = DataFiltering.FilterMaterialsByManufacturers(storagesMaterialsAll, constructionOption);
				var constructionMaterialSets = GetMaterialsSetsWithConstructionTypes(materialSets, dbContext, constructionTypeId);
				BuildType allowedBuildType = constructionOption.Filter.BuildType;

				foreach (var constructionMaterialSet in constructionMaterialSets)
				{
					var constructionUnits = constructionMaterialSet.Value;
					bool isAssemblyBuildRequired = !objectToDeliver.ContainsAssemblyShop && (BuildType)constructionUnits[0].ConstructionUnitTypeId == BuildType.Block;

					if (allowedBuildType != BuildType.NoMatter && (BuildType)constructionUnits[0].ConstructionUnitTypeId != allowedBuildType)
						continue;

					int[] uniqueConstructionUnitIds = constructionUnits.Select(cUnit => cUnit.ConstructionUnitId).ToArray();
					var constructionUnitsFromStorage = storagesMaterials.Where(sm => uniqueConstructionUnitIds.Contains(sm.ConstructionUnitId)).ToList();

					int[] uniqueStorageIds = constructionUnitsFromStorage.Select(cUnit => cUnit.StorageId).Distinct().ToArray();
					MaterialParams?[,] storageMaterialMatrix = new MaterialParams?[uniqueStorageIds.Length, uniqueConstructionUnitIds.Length];

					for (int storageId = 0; storageId < uniqueStorageIds.Length; storageId++)
						for (int materialId = 0; materialId < uniqueConstructionUnitIds.Length; materialId++)
						{
							var constructionUnitFromStorage = constructionUnitsFromStorage
								.FirstOrDefault(cUnit =>
								cUnit!.ConstructionUnitId == uniqueConstructionUnitIds[materialId]
								&& cUnit.StorageId == uniqueStorageIds[storageId], null);

							storageMaterialMatrix[storageId, materialId] = (constructionUnitFromStorage == null)
								? null
								: new MaterialParams()
								{
									PricePerUnit = constructionUnitFromStorage.PricePerUnit,
									Amount = (decimal)constructionUnits[materialId].Amount
								};
						}

					var storagesManufacturer = GetManufacturersByStorageIds(uniqueStorageIds, dbContext, dataTuple);

					var orderVariants = CalculateOrderVariants(storageMaterialMatrix, uniqueStorageIds, storagesManufacturer);
					SortCostAndTimeListByFilterMethod(orderVariants, constructionOption.Filter.TargetMark);

					response.Orders.Add(await GetOrderVariantsWithInfoAsync(orderVariants, dataTuple, constructionUnits, storagesManufacturer, isAssemblyBuildRequired, constructionOption.ConstructionId, dbContext));
				}
			}
			await context.Response.WriteAsJsonAsync(response);
		}

		internal static async Task CalculateSimpleOrderCostTime(HttpContext context, ApplicationContext dbContext, DistanceService distanceService, CancellationToken cancellationToken)
		{
            try
            {
                // Чтение тела запроса
                var algorithmRequest = await context.Request.ReadFromJsonAsync<AlgorithmRequest>(cancellationToken);
                // Проверка, что тело запроса не пустое
                if (algorithmRequest == null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = "Пустое тело запроса!" }, cancellationToken);
                    return;
                }

                // Проверка валидности полей AlgorithmRequest
                if (!IsValidAlgorithmRequest(algorithmRequest))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = "Некорректные данные запроса" }, cancellationToken);
                    return;
                }

                // Создание сервиса алгоритма
                var algorithmService = new AlgorithmService(await LoadData(dbContext, distanceService), dbContext);

                // Выполнение алгоритма
                var results = await algorithmService.GetAlgorithmSolve(algorithmRequest, cancellationToken);

                // Возврат успешного результата
                await context.Response.WriteAsJsonAsync(results, cancellationToken);
            }
            catch (Exception ex)
            {
                // Логирование ошибки и возврат 500
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message }, cancellationToken);
            }
        }

        private static bool IsValidAlgorithmRequest(AlgorithmRequest request)
        {
            // Проверяем, есть ли ConstructionOptions
            if (request.Data == null || !request.Data.Any())
                return false;

            // Проходим по каждому ConstructionOption и проверяем его валидность
            foreach (var option in request.Data)
            {
                if (option.ConstructionId <= 0)
                    return false;

                if (option.Filter == null)
                    return false;

                // Проверяем валидность полей Filter
                if (!IsValidFilter(option.Filter))
                    return false;
            }

            return true;
        }

        private static bool IsValidFilter(Filter filter)
        {
            //if (filter.FilterMethod == null || filter.BuildType == null)
            //    return false;

            //if (filter.TransportTypeIds == null || !filter.TransportTypeIds.Any())
            //    return false;

            //// Проверяем валидность других полей фильтра
            //if (filter.CertainLogists == null || filter.CertainManufacturers == null)
            //    return false;

            return true;
        }

        static List<ShortOrderVariant> CalculateOrderVariants(MaterialParams?[,] storageMaterialMatrix, int[] storageIds, Dictionary<int, Manufacturer> storagesManufacturer)
		{
			///алгоритм для перебора, к сожалению, всех вариантов
			int storagesCount = storageMaterialMatrix.GetLength(0);
			int materialsCount = storageMaterialMatrix.GetLength(1);

			int[] storageIndices = new int[materialsCount];
			List<ShortOrderVariant> orderVariants = new();

			while (storageIndices.Last() < storagesCount)
			{
				bool variantIsValid = true;
				ShortOrderVariant orderVariant = new ShortOrderVariant(storageIndices.ToArray(), new DeliveryVariant[materialsCount], new decimal[materialsCount], new decimal[materialsCount], 0, 0);
				List<int> storagesWithCalculatedDeliveries = new();

				for (int materialIndex = 0; materialIndex < storageIndices.Length; materialIndex++)
				{
					MaterialParams? materialVariant = storageMaterialMatrix[storageIndices[materialIndex], materialIndex];

					if (materialVariant is null)
					{
						variantIsValid = false;
						break;
					}

					FillOrderVariantWithMaterial(orderVariant, materialIndex, materialVariant);

					int storageId = storageIds[storageIndices[materialIndex]];
					orderVariant.DeliveryVariants[materialIndex] = DeliveryVariants.FirstOrDefault(c => c.StorageId == storageId && ValidateLogisticCompany(storagesManufacturer, c));

					if (orderVariant.DeliveryVariants[materialIndex] is null)
					{
						variantIsValid = false;
						break;
					}

					if (orderVariant.DeliveryVariants[materialIndex].DeliveryTime > orderVariant.DeliveryTime)
						orderVariant.DeliveryTime = orderVariant.DeliveryVariants[materialIndex].DeliveryTime;

					if (!storagesWithCalculatedDeliveries.Contains(storageIndices[materialIndex]))
					{
						orderVariant.Cost += orderVariant.DeliveryVariants[materialIndex].Cost;
						storagesWithCalculatedDeliveries.Add(storageIndices[materialIndex]);
					}

					orderVariant.StorageIdsForMaterials[materialIndex] = storageIndices[materialIndex];
				}

				if (variantIsValid)
					orderVariants.Add(orderVariant);

				UpdateIndices(storageIndices, materialsCount);
			}
			return orderVariants;
		}

		static bool ValidateLogisticCompany(Dictionary<int, Manufacturer> storagesManufacturer, DeliveryVariant deliveryVariant) =>
			deliveryVariant.DeliveryRouteParams.All(d => d.TransportOnFleet.CompanyTypeId == (int)CompanyType.LogisticCompany || storagesManufacturer[deliveryVariant.StorageId].ManufacturerId == d.TransportOnFleet.CompanyId);

		static void FillOrderVariantWithMaterial(ShortOrderVariant orderVariant, int materialIndex, MaterialParams materialVariant)
		{
			orderVariant.MaterialCosts[materialIndex] = materialVariant.Cost;
			orderVariant.MaterialPricesPerUnit[materialIndex] = materialVariant.PricePerUnit;
			orderVariant.Cost += materialVariant.Cost;
		}

		static void UpdateIndices(int[] storageIndices, int materialsCount)
		{
			storageIndices[0]++;
			
			for (int materialIndex = 0; storageIndices[materialIndex] == materialsCount && materialIndex < storageIndices.Length - 1; materialIndex++)
			{
				storageIndices[materialIndex] = 0;
				storageIndices[materialIndex + 1]++;
			}
		}
	}
}
