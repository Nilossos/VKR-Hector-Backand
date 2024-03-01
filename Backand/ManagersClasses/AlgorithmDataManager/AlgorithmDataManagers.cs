using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;
using Microsoft.EntityFrameworkCore;
using Backand.Services;
using Backand.ManagersClasses.AlgorithmDataManager.TrackGetters;
using Backand.FrontendEntities.AlgorithmResponse;
using Backand.DbEntities.ConstructionSpace;
using System.Collections.Generic;
using OpenQA.Selenium.DevTools.V118.Network;
using static OpenQA.Selenium.VirtualAuth.VirtualAuthenticatorOptions;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.Design;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class AlgorithmDataManagers
	{
		private static List<DeliveryVariant> DeliveryVariants { get; set; } = null!;

		private static async Task<List<StorageMaterial>> GetStoragesMaterialsAsync(ApplicationContext dbContext) =>
			await (from storConstrUnit in dbContext.Storage_ConstructionUnit
						  join storage in dbContext.Storage on storConstrUnit.StorageId equals storage.StorageId
						  join constrUnit in dbContext.ConstructionUnit on storConstrUnit.ConstructionUnitId equals constrUnit.ConstructionUnitId
						  join manufacturer in dbContext.Manufacturer on storage.ManufacturerId equals manufacturer.ManufacturerId
						  select new StorageMaterial
						  {
							  StorageId = storage.StorageId,
							  StorageName = storage.Name,
							  ConstructionUnitId = constrUnit.ConstructionUnitId,
							  ConstructionUnitTypeId = constrUnit.ConstructionUnitTypeId,
							  ManufacturerId = manufacturer.ManufacturerId,
							  PricePerUnit = storConstrUnit.PricePerUnit
						  }).ToListAsync();

		private static async Task<List<TransportOnFleetWithRegions>> GetTransportsOnFleetsAsync(ApplicationContext dbContext) =>
			await (from deliveryRegion in dbContext.DeliveryRegion
						  join trOnFleet in dbContext.TransportFleet_Transport on deliveryRegion.TransportFleet_TransportId equals trOnFleet.TransportFleet_TransportId
						  join transport in dbContext.Transport on trOnFleet.TransportId equals transport.TransportId
						  join trMode in dbContext.TransportMode on transport.TransportModeId equals trMode.TransportModeId
						  join trType in dbContext.TransportType on trMode.TransportTypeId equals trType.TransportTypeId
						  join trFleet in dbContext.TransportFleet on trOnFleet.TransportFleetId equals trFleet.TransportFleetId
						  join company in dbContext.Company on trFleet.CompanyId equals company.CompanyId
						  join companyNames in (await dbContext.LogisticCompany.ToListAsync()).Select(c => new { CompanyId = c.LogisticCompanyId, CompanyName = c.Name })
										.Union((await dbContext.Manufacturer.ToListAsync()).Select(c => new { CompanyId = c.ManufacturerId, CompanyName = c.Name }))
						  on company.CompanyId equals companyNames.CompanyId
						  join coefType in dbContext.CoefficientType on trOnFleet.CoefficientTypeId equals coefType.CoefficientTypeId
						  select new
						  {
							  deliveryRegion.RegionId,
							  TransportOnFleet = new TransportOnFleet
							  {
								  TransportId = transport.TransportId,
								  TransportName = transport.Name,
								  TransportFleet = trFleet,
								  CoefficientTypeId = trOnFleet.CoefficientTypeId,
								  CoefficientTypeName = coefType.Name,
								  CoefficientValue = trOnFleet.CoefficientValue,
								  TransportTypeId = trType.TransportTypeId,
								  TransportTypeName = trType.Name,
								  TransportModeName = trMode.Name,
								  AverageSpeed = trOnFleet.AverageSpeed,
								  CompanyId = company.CompanyId,
								  CompanyName = companyNames.CompanyName,
								  CompanyTypeId = company.CompanyTypeId
							  }
						  })
			 .GroupBy(t => t.TransportOnFleet)
			 .Select(g => new TransportOnFleetWithRegions
			 {
				 TransportOnFleet = g.Key,
				 RegionIds = g.Select(r => (int)r.RegionId!).ToArray()
			 })
			 .ToListAsync();

		//DEBUG
		private static async Task<dynamic> GetTransportsOnFleetsAsyncTest(ApplicationContext dbContext)
		{
			var companyNamesQuery = (from logCompany in dbContext.LogisticCompany select new { CompanyId = logCompany.LogisticCompanyId, CompanyName = logCompany.Name })
				.Union(from manufact in dbContext.Manufacturer select new { CompanyId = manufact.ManufacturerId, CompanyName = manufact.Name });

			var query = (from deliveryRegion in dbContext.DeliveryRegion
							   join trOnFleet in dbContext.TransportFleet_Transport on deliveryRegion.TransportFleet_TransportId equals trOnFleet.TransportFleet_TransportId
							   join transport in dbContext.Transport on trOnFleet.TransportId equals transport.TransportId
							   join trMode in dbContext.TransportMode on transport.TransportModeId equals trMode.TransportModeId
							   join trType in dbContext.TransportType on trMode.TransportTypeId equals trType.TransportTypeId
							   join trFleet in dbContext.TransportFleet on trOnFleet.TransportFleetId equals trFleet.TransportFleetId
							   join company in dbContext.Company on trFleet.CompanyId equals company.CompanyId
							   join companyName in companyNamesQuery on company.CompanyId equals companyName.CompanyId
							   join coefType in dbContext.CoefficientType on trOnFleet.CoefficientTypeId equals coefType.CoefficientTypeId
							   select new
							   {
								   deliveryRegion.RegionId,
								   TransportOnFleet = new TransportOnFleet
								   {
									   TransportId = transport.TransportId,
									   TransportName = transport.Name,
									   TransportFleet = trFleet,
									   CoefficientTypeId = trOnFleet.CoefficientTypeId,
									   CoefficientTypeName = coefType.Name,
									   CoefficientValue = trOnFleet.CoefficientValue,
									   TransportTypeId = trType.TransportTypeId,
									   TransportTypeName = trType.Name,
									   TransportModeName = trMode.Name,
									   AverageSpeed = trOnFleet.AverageSpeed,
									   CompanyId = company.CompanyId,
									   CompanyName = companyName.CompanyName,
									   CompanyTypeId = company.CompanyTypeId
								   }
							   }).GroupBy(t => t.TransportOnFleet).ToListAsync();
			//.Select(g => new TransportOnFleetWithRegions
			//{
			//	TransportOnFleet = g.Key,
			//	RegionIds = g.Select(r => (int)r.RegionId!).ToArray()
			//})
			//.ToListAsync();

			

			return query;

			//join companyNames in (await dbContext.LogisticCompany.ToListAsync()).Select(c => new { CompanyId = c.LogisticCompanyId, CompanyName = c.Name })
			// .Union((await dbContext.Manufacturer.ToListAsync()).Select(c => new { CompanyId = c.ManufacturerId, CompanyName = c.Name })).ToList()
			//on company.CompanyId equals companyNames.CompanyId


		}

		private static Dictionary<int, List<ConstructionUnitSupplemented>> GetMaterialsSetsWithConstructionTypes(List<MaterialSet> materialSets, ApplicationContext dbContext, int constructionTypeId) =>
			(from mSet in materialSets
					where mSet.ConstructionTypeId == constructionTypeId
					join mSet_cUnit in dbContext.MaterialSet_ConstructionUnit on mSet.MaterialSetId equals mSet_cUnit.MaterialSetId
					join cUnit in dbContext.ConstructionUnit on mSet_cUnit.ConstructionUnitId equals cUnit.ConstructionUnitId
					join cUnitType in dbContext.ConstructionUnitType on cUnit.ConstructionUnitTypeId equals cUnitType.ConstructionUnitTypeId
					join measureUnit in dbContext.MeasureUnit on cUnit.MeasureUnitId equals measureUnit.MeasureUnitId
					select new
					{
						mSet.MaterialSetId,
						ConstructionUnitWithAmount = new ConstructionUnitSupplemented
						{
							ConstructionUnitId = cUnit.ConstructionUnitId,
							ConstructionUnitTypeId = cUnit.ConstructionUnitTypeId,
							MeasureUnitId = cUnit.MeasureUnitId,
							Name = cUnit.Name,
							TypeName = cUnitType.Name,
							MeasureUnitName = measureUnit.Name,
							Amount = mSet_cUnit.Amount
						}
					})
			.GroupBy(m => m.MaterialSetId)
			.ToDictionary(
				group => group.Key,
				group => group.Select(g => g.ConstructionUnitWithAmount).ToList()
			);

		private static List<TransportOnFleetWithRegions> FilterFleetsByLogisticCompanies(List<TransportOnFleetWithRegions> transportsOnFleets, ConstructionOption constructionOption)
		{
			if (constructionOption.Filter.CertainManufacturers.Ids.Count > 0)
				return transportsOnFleets
					.Where(t => constructionOption.Filter.CertainLogists.Ids.Contains(t.TransportOnFleet!.CompanyId))
					.ToList();
			else
				return transportsOnFleets;
		}

		static List<StorageMaterial> FilterMaterialsByManufacturers(List<StorageMaterial> storagesMaterials, ConstructionOption constructionOption)
		{
			if (constructionOption.Filter.CertainLogists.Ids.Count > 0)
				return storagesMaterials
					.Where(m => constructionOption.Filter.CertainManufacturers.Ids.Contains(m.ManufacturerId))
					.ToList();
			else
				return storagesMaterials;
		}

		static async Task<AlgorithmData> LoadData(ApplicationContext dbContext, DistanceService distanceService)
		{
			List<Construction> constructions = await dbContext.Construction.Include(c => c.Object).ToListAsync();
			List<Storage> storages = await dbContext.Storage.ToListAsync();
			List<TransportFleet> transportFleets = await dbContext.TransportFleet.ToListAsync();

			//DEBUG
			var what = await GetTransportsOnFleetsAsyncTest(dbContext);
			await Console.Out.WriteLineAsync("TransportOnFleetAll Test Complete");

			List<TransportOnFleetWithRegions> transportsOnFleetsAll = new(); //await GetTransportsOnFleetsAsync(dbContext);
			List<MaterialSet> materialSets = await dbContext.MaterialSet.ToListAsync();
			List<StorageMaterial> storagesMaterialsAll = await GetStoragesMaterialsAsync(dbContext);

			TransportFleetToObjectTracker transportFleetToObject = new(dbContext,distanceService);
			StorageToObjectTracker storageToObject = new(dbContext,distanceService);
			StorageToTransportFleetTracker storageToFleet = new(dbContext,distanceService);

			List<StorageToObjectsDistance> storageToObjectsDistances = await storageToObject.GetTrackToEndpoints();
			List<StorageToTransportFleetDistance> storageToTransportFleetDistances = await storageToFleet.GetTrackToEndpoints();
			List<TransportFleetToObjectsDistance> transportFleetToObjectsDistance = await transportFleetToObject.GetTrackToEndpoints();
			return new AlgorithmData(constructions, storages, transportFleets, transportsOnFleetsAll, materialSets, storagesMaterialsAll, storageToObjectsDistances, storageToTransportFleetDistances, transportFleetToObjectsDistance);
		}

		static void FillDeliveryVariants(AlgorithmData data, ConstructionOption constructionOption, Objects? objectsToDeliver)
		{
			DeliveryVariants.Clear();

			List<TransportOnFleetWithRegions> transportsOnFleets = FilterFleetsByLogisticCompanies(data.transportsOnFleetsAll, constructionOption);
			FilterMethod filterMethod = constructionOption.Filter.FilterMethod;

			AddDeliveryVariantsForGroundTransport(data, objectsToDeliver, transportsOnFleets, filterMethod);
			AddDeliveryVariantsForAirTransport(objectsToDeliver, transportsOnFleets, filterMethod);
			SortCostAndTimeListByFilterMethod(DeliveryVariants, filterMethod);
		}

		static void AddDeliveryVariantsForGroundTransport(AlgorithmData data, Objects objectsToDeliver, List<TransportOnFleetWithRegions> transportsOnFleets, FilterMethod filterMethod)
		{
			Dictionary<int, decimal?> storageToCertainObjectsDistances = GetStorageToCertainObjectsDistances(data.storageToObjectsDistances, objectsToDeliver);

			foreach (var transportFleet in data.transportFleets)
			{
				int transportFleetId = transportFleet.TransportFleetId;
				var transportsOnFleet = FilterTransportFromTransportFleetByType(transportsOnFleets, transportFleetId, TransportTypeValue.Ground);
				SortTransportsOnFleetByFilter(transportsOnFleet, filterMethod);
				var transports = GetFirstTransportFromFleets(transportsOnFleets);

				foreach (var storage in data.storages)
				{
					int storageId = storage.StorageId;

					foreach (var transport in transports)
						if (transport.RegionIds.Contains((int)storage.RegionId!) && transport.RegionIds.Contains(objectsToDeliver!.RegionId))
						{
							decimal distance = (decimal)(data.storageToTransportFleetDistances.FirstOrDefault(d => d.TransportFleetId == transportFleetId && d.StorageId == storageId)!.Distance + storageToCertainObjectsDistances[storageId])!;
							decimal cost = (decimal)transport.TransportOnFleet!.CoefficientValue * distance;
							decimal deliveryTime = distance / (decimal)transport.TransportOnFleet!.AverageSpeed;

							DeliveryVariants.Add(new DeliveryVariant
							{
								StorageId = storageId,
								Storage = storage,
								DeliveryRouteParams = new List<DeliveryRouteParams>(){ new DeliveryRouteParams
								{
									TransportFleetId = transportFleetId,
									TransportOnFleet = transport.TransportOnFleet,
									Cost = cost,
									DeliveryTime = deliveryTime,
									Distance = distance
								}},
								Cost = cost,
								DeliveryTime = deliveryTime
							});
							break;
						}
				}
			}
		}

		static void SortTransportsOnFleetByFilter(List<TransportOnFleetWithRegions> transportsOnFleet, FilterMethod filterMethod)
		{
			switch(filterMethod)
			{
				case FilterMethod.Time:
					transportsOnFleet.Sort((t2, t1) => t1.TransportOnFleet.AverageSpeed.CompareTo(t2.TransportOnFleet.AverageSpeed));
					break;
				case FilterMethod.Money:
					transportsOnFleet.Sort((t1, t2) => t1.TransportOnFleet.CoefficientValue.CompareTo(t2.TransportOnFleet.CoefficientValue));
					break;
				case FilterMethod.Balanced:
					transportsOnFleet
						.Sort((t1, t2) => (t1.TransportOnFleet.CoefficientValue / t1.TransportOnFleet.AverageSpeed)
							.CompareTo(t2.TransportOnFleet.CoefficientValue / t2.TransportOnFleet.AverageSpeed));
					break;
			}
		}

		static List<TransportOnFleetWithRegions> GetTransportToDeliverFromFleet(List<TransportOnFleetWithRegions> transportsOnFleets, Objects objectsToDeliver, TransportTypeValue transportType) =>
			transportsOnFleets
				.Where(t => t.RegionIds.Contains(objectsToDeliver.RegionId) && t.TransportOnFleet.TransportTypeId == (int)transportType)
				.ToList();

		static List<TransportOnFleetWithRegions> GetGroundTransportToDeliverInAnyRegion(List<TransportOnFleetWithRegions> transportsOnFleets, int[] regionIds) =>
			transportsOnFleets.Where(t => t.TransportOnFleet.TransportTypeId == (int)TransportTypeValue.Ground && t.RegionIds.Any(r  => regionIds.Contains(r!))).ToList();

		static TransportOnFleetWithRegions? GetFirstTransportToDeliverInRegion(List<TransportOnFleetWithRegions> transportsOnFleets, int regionId) =>
			transportsOnFleets.FirstOrDefault(t => t.RegionIds.Contains(regionId));

		static List<TransportOnFleetWithRegions> GetFirstTransportFromFleets(List<TransportOnFleetWithRegions> transportsOnFleets) =>
			transportsOnFleets.DistinctBy(t => t.TransportOnFleet.TransportFleet.TransportFleetId).ToList();

		static void AddDeliveryVariantsForAirTransport(Objects objectsToDeliver, List<TransportOnFleetWithRegions> transportsOnFleets, FilterMethod filterMethod)
		{
			List<TransportOnFleetWithRegions> airTransport = GetTransportToDeliverFromFleet(transportsOnFleets, objectsToDeliver, TransportTypeValue.Air);
			SortTransportsOnFleetByFilter(airTransport, filterMethod);
			var airTransportFiltered = GetFirstTransportFromFleets(airTransport);

			int[] regionIds = airTransportFiltered.Select(t => (int)t.TransportOnFleet.TransportFleet.RegionId!).ToArray();
			var groundTransport = GetGroundTransportToDeliverInAnyRegion(airTransportFiltered, regionIds);
			SortTransportsOnFleetByFilter(groundTransport, filterMethod);
			var groundTransportFiltered = GetFirstTransportFromFleets(groundTransport);

			//составление вариантов
			GetGroundAirTransportVariations(airTransportFiltered, groundTransportFiltered);

			//последующая сортировка
		}

		static void GetGroundAirTransportVariations(List<TransportOnFleetWithRegions> airTransports, List<TransportOnFleetWithRegions> groundTransports)
		{
			foreach (var airTransport in airTransports)
			{
				var airRegionId = airTransport.TransportOnFleet.TransportFleet.TransportFleetId;
				var groundTransport = GetFirstTransportToDeliverInRegion(groundTransports, airRegionId);
				if (groundTransport is null)
					continue;


			}
		}

		static Dictionary<int, decimal?> GetStorageToCertainObjectsDistances(List<StorageToObjectsDistance> storageToObjectsDistances, Objects? objectsToDeliver) =>
			storageToObjectsDistances
				.Where(d => d.ObjectsId == objectsToDeliver!.ObjectsId)
				.ToDictionary(d => d.StorageId, d => d.Distance);

		static List<TransportOnFleetWithRegions> FilterTransportFromTransportFleetByType(List<TransportOnFleetWithRegions> transportsOnFleets, int transportFleetId, TransportTypeValue transportTypeValue) =>
			transportsOnFleets
			.Where(
				t => t.TransportOnFleet.TransportFleet.TransportFleetId == transportFleetId
				&& t.TransportOnFleet.TransportTypeId == (int)transportTypeValue
			).ToList();

		static void SortCostAndTimeListByFilterMethod<T>(List<T> deliveryCosts, FilterMethod filterMethod) where T : ICostAndTime
		{
			switch (filterMethod)
			{
				case FilterMethod.Money:
					deliveryCosts.Sort((e1, e2) => e1.Cost.CompareTo(e2.Cost));
					break;
				case FilterMethod.Time:
					deliveryCosts.Sort((e1, e2) => e1.DeliveryTime.CompareTo(e2.DeliveryTime));
					break;
				case FilterMethod.Balanced:
					deliveryCosts.Sort((e1, e2) => (e1.Cost * e1.DeliveryTime).CompareTo(e1.Cost * e1.DeliveryTime));
					break;
				default:
					throw new NotImplementedException("Обработка такого значения фильтрации не была предусмотрена");
			};
		}

		public static async Task CalculateOrderCostTime(HttpContext context, ApplicationContext dbContext, DistanceService distanceService)
		{
			AlgorithmRequest? algorithmRequest = await context.Request.ReadFromJsonAsync<AlgorithmRequest>() ?? throw new ArgumentNullException("Пустое тело запроса!");
			List<ConstructionOption> constructionOptions = algorithmRequest.ConstructionOptions;

			AlgorithmData dataTuple = await LoadData(dbContext, distanceService);
			var (constructions, storages, transportFleets, transportsOnFleetsAll, materialSets, storagesMaterialsAll, storageToObjectsDistances, storageToTransportFleetDistance, transportFleetToObjectsDistance) = dataTuple;

			AlgorithmResponse response = new();

			foreach (var constructionOption in constructionOptions)
			{
				(Objects? objectsToDeliver, int constructionTypeId) = constructions
					.Where(c => c.ConstructionId == constructionOption.ConstructionId)
					.Select(c => (c.Object, c.ConstructionTypeId))
					.FirstOrDefault();

				FillDeliveryVariants(dataTuple, constructionOption, objectsToDeliver);

				List<StorageMaterial> storagesMaterials = FilterMaterialsByManufacturers(storagesMaterialsAll, constructionOption);
				var constructionMaterialSets = GetMaterialsSetsWithConstructionTypes(materialSets, dbContext, constructionTypeId);
				BuildType allowedBuildType = constructionOption.Filter.BuildType;

				foreach (var constructionMaterialSet in constructionMaterialSets)
				{
					var constructionUnits = constructionMaterialSet.Value;

					//пропускаем тип постройки, который отключен фильтром
					if (allowedBuildType != BuildType.NoMatter && (BuildType)constructionUnits[0].ConstructionUnitTypeId == allowedBuildType)
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

					var orderVariants = CalculateOrderVariants(storageMaterialMatrix, uniqueStorageIds);
					SortCostAndTimeListByFilterMethod(orderVariants, constructionOption.Filter.FilterMethod);

					response.Orders.Add(GetOrderVariantsWithInfo(orderVariants, dataTuple, constructionUnits, manufaturerNames: GetManufacturersNamesByStorageIds(uniqueStorageIds, dbContext, dataTuple)));
				}
			}

			await context.Response.WriteAsJsonAsync(response);
		}

		static Order GetOrderVariantsWithInfo(List<ShortOrderVariant> orderVariants, AlgorithmData data, List<ConstructionUnitSupplemented> constructionUnits, Dictionary<int, string> manufaturerNames)
		{
			Order order = new();

			for (int i = 0; i < orderVariants.Count; i++)
			{
				var orderVariant = orderVariants[i];

				OrderResult orderResult = new(orderVariant.Cost, orderVariant.DeliveryTime);

				List<MaterialOrderVariant> materialOrderVariants = new();

				for (int constrUnitIndex = 0; constrUnitIndex < constructionUnits.Count; constrUnitIndex++)
				{
					var deliveryVariant = orderVariant.DeliveryVariants[constrUnitIndex];

					ConstructionUnitSupplemented constructionUnit = constructionUnits[constrUnitIndex];
					Storage storage = data.storages.FirstOrDefault(s => s.StorageId == orderVariant.StorageIdsForMaterials[constrUnitIndex])!;

					BuildInfo buildInfo = new(constructionUnit.Name, constructionUnit.TypeName, constructionUnit.MeasureUnitName, constructionUnit.Amount);
					ProductionInfo productionInfo = new(manufaturerNames[storage.StorageId], storage.Name, storage.Address, orderVariant.MaterialPricesPerUnit[constrUnitIndex], orderVariant.MaterialCosts[constrUnitIndex]);
					List<LogisticInfo> logisticInfo = LogisticInfo.CreateLogisticInfoList(deliveryVariant.DeliveryRouteParams);

					materialOrderVariants.Add(new(buildInfo, productionInfo, logisticInfo));
				}
				OrderVariant filledOrderVariant = new(materialOrderVariants, orderResult);

				order.Variants.Add(filledOrderVariant);
			}
			return order;
		}

		static Dictionary<int, string> GetManufacturersNamesByStorageIds(int[] storageIds, ApplicationContext dbContext, AlgorithmData data) =>
			data.storages
				.Where(s => storageIds.Contains(s.StorageId))
				.Join(dbContext.Manufacturer,
				s => s.ManufacturerId,
				m => m.ManufacturerId,
				(s, m) => new { s.StorageId, ManufacturerName = m.Name })
				.ToDictionary(o => o.StorageId, o => o.ManufacturerName);

		static List<ShortOrderVariant> CalculateOrderVariants(MaterialParams?[,] storageMaterialMatrix, int[] storageIds)
		{
			//алгоритм для перебора, к сожалению, всех вариантов
			int storagesCount = storageMaterialMatrix.GetLength(0);
			int materialsCount = storageMaterialMatrix.GetLength(1);

			int[] storageIndicies = new int[materialsCount];
			List<ShortOrderVariant> orderVariants = new();

			while (storageIndicies.Last() < storagesCount)
			{
				bool variantIsValid = true;
				ShortOrderVariant orderVariant = new ShortOrderVariant(storageIndicies, new DeliveryVariant[materialsCount], new decimal[materialsCount], new decimal[materialsCount], 0, 0);

				for (int materialIndex = 0; materialIndex < storageIndicies.Length; materialIndex++)
				{
					MaterialParams? materialVariant = storageMaterialMatrix[storageIndicies[materialIndex], materialIndex];

					if (materialVariant is null)
					{
						variantIsValid = false;
						break;
					}

					orderVariant.MaterialCosts[materialIndex] = materialVariant.Cost;
					orderVariant.MaterialPricesPerUnit[materialIndex] = materialVariant.PricePerUnit;

					orderVariant.Cost += materialVariant.Cost;

					orderVariant.DeliveryVariants[materialIndex] = DeliveryVariants.FirstOrDefault(c => c.StorageId == storageIds[storageIndicies[materialIndex]]);

					if (orderVariant.DeliveryVariants[materialIndex] is null)
					{
						variantIsValid = false;
						break;
					}

					if (orderVariant.DeliveryVariants[materialIndex].DeliveryTime > orderVariant.DeliveryTime)
						orderVariant.DeliveryTime = orderVariant.DeliveryVariants[materialIndex].DeliveryTime;

					if (!orderVariant.StorageIdsForMaterials.Contains(storageIndicies[materialIndex]))
						orderVariant.Cost += orderVariant.DeliveryVariants[materialIndex].Cost;

					orderVariant.StorageIdsForMaterials[materialIndex] = storageIndicies[materialIndex];
				}

				if (variantIsValid)
					orderVariants.Add(orderVariant);

				storageIndicies[0]++;

				for (int materialIndex = 0; storageIndicies[materialIndex] == materialsCount && materialIndex < storageIndicies.Length - 1; materialIndex++)
				{
					storageIndicies[materialIndex] = 0;
					storageIndicies[materialIndex + 1]++;
				}
			}
			return orderVariants;
		}
	}
}
