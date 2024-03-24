using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;
using Microsoft.EntityFrameworkCore;
using Backand.Services;
using Backand.ManagersClasses.AlgorithmDataManager.TrackGetters;
using Backand.FrontendEntities.AlgorithmResponse;
using Backand.DbEntities.ConstructionSpace;
using static Backand.ManagersClasses.AlgorithmDataManager.InnerDataRequests;
using static Backand.ManagersClasses.AlgorithmDataManager.DataFiltering;
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

			TransportFleetToObjectTracker transportFleetToObject = new(dbContext,distanceService);
			StorageToObjectsTracker storageToObjects = new(dbContext,distanceService);
			StorageToTransportFleetTracker storageToFleet = new(dbContext,distanceService);

			List<StorageToObjectsDistance> storageToObjectsDistances = await storageToObjects.GetTrackToEndpoints();
			List<StorageToTransportFleetDistance> storageToTransportFleetDistances = await storageToFleet.GetTrackToEndpoints();
			List<TransportFleetToObjectsDistance> transportFleetToObjectsDistance = await transportFleetToObject.GetTrackToEndpoints();
			return new AlgorithmData(constructions, storages, transportFleets, transportsOnFleetsAll, materialSets, storagesMaterialsAll, storageToObjectsDistances, storageToTransportFleetDistances, transportFleetToObjectsDistance);
		}

		internal static async Task CalculateOrderCostTime(HttpContext context, ApplicationContext dbContext, DistanceService distanceService)
		{
			AlgorithmRequest? algorithmRequest = await context.Request.ReadFromJsonAsync<AlgorithmRequest>() ?? throw new ArgumentNullException("Пустое тело запроса!");
			List<ConstructionOption> constructionOptions = algorithmRequest.ConstructionOptions;

			AlgorithmData dataTuple = await LoadData(dbContext, distanceService);
			var (constructions, storages, transportFleets, transportsOnFleetsAll, materialSets, storagesMaterialsAll, storageToObjectsDistances, storageToTransportFleetDistance, transportFleetToObjectsDistance) = dataTuple;

			AlgorithmResponse response = new();

			foreach (var constructionOption in constructionOptions)
			{
                (Objects objectsToDeliver, int constructionTypeId) = constructions
					.Where(c => c.ConstructionId == constructionOption.ConstructionId)
					.Select(c => (c.Object!, c.ConstructionTypeId))
					.First();

                FillDeliveryVariants(dataTuple, constructionOption, objectsToDeliver);

				List<StorageMaterial> storagesMaterials = FilterMaterialsByManufacturers(storagesMaterialsAll, constructionOption);
				var constructionMaterialSets = GetMaterialsSetsWithConstructionTypes(materialSets, dbContext, constructionTypeId);
				BuildType allowedBuildType = constructionOption.Filter.BuildType;

				foreach (var constructionMaterialSet in constructionMaterialSets)
				{
                    var constructionUnits = constructionMaterialSet.Value;
                    bool isAssemblyBuildRequired = !objectsToDeliver.ContainsAssemblyShop && (BuildType)constructionUnits[0].ConstructionUnitTypeId == BuildType.Block;

					//пропускаем тип постройки, который отключен фильтром
					if (allowedBuildType != BuildType.NoMatter && (BuildType)constructionUnits[0].ConstructionUnitTypeId != allowedBuildType)
						continue;

					int[] uniqueConstructionUnitIds = constructionUnits.Select(cUnit => cUnit.ConstructionUnitId).ToArray();
					var constructionUnitsFromStorage = storagesMaterials.Where(sm => uniqueConstructionUnitIds.Contains(sm.ConstructionUnitId)).ToList();

					int[] uniqueStorageIds = constructionUnitsFromStorage.Select(cUnit => cUnit.StorageId).Distinct().ToArray();
					MaterialParams?[,] storageMaterialMatrix = new MaterialParams?[uniqueStorageIds.Length, uniqueConstructionUnitIds.Length];
					MaterialParams? materialVariant = null;

					for (int storageId = 0; storageId < uniqueStorageIds.Length; storageId++)
						for (int materialId = 0; materialId < uniqueConstructionUnitIds.Length; materialId++)
						{
							var constructionUnitFromStorage = constructionUnitsFromStorage
								.FirstOrDefault(cUnit =>
								cUnit!.ConstructionUnitId == uniqueConstructionUnitIds[materialId]
								&& cUnit.StorageId == uniqueStorageIds[storageId], null);

							if (constructionUnitFromStorage is not null)
								materialVariant = new MaterialParams
								{
									PricePerUnit = constructionUnitFromStorage.PricePerUnit,
									Amount = (decimal)constructionUnits[materialId].Amount,
								};
							storageMaterialMatrix[storageId, materialId] = materialVariant;
						}

					var storagesManufacturer = GetManufacturersByStorageIds(uniqueStorageIds, dbContext, dataTuple);

					var orderVariants = CalculateOrderVariants(storageMaterialMatrix, uniqueStorageIds, storagesManufacturer);
					SortCostAndTimeListByFilterMethod(orderVariants, constructionOption.Filter.FilterMethod);

					response.Orders.Add(GetOrderVariantsWithInfo(orderVariants, dataTuple, constructionUnits, storagesManufacturer, isAssemblyBuildRequired));
				}
			}

			await context.Response.WriteAsJsonAsync(response);
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
