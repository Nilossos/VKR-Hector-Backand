using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;
using Microsoft.EntityFrameworkCore;
using Backand.Services;
using Backand.FrontendEntities;
using Backand.ManagersClasses.AlgorithmDataManager.TrackGetters;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
    public class AlgorithmDataManagers
    {
        private static List<DeliveryParamsUnit> DeliveryVariants { get; set; }

		private static async Task<List<StorageMaterial>> GetStoragesMaterialsAsync(ApplicationContext dbContext)
        {
            return await (from storConstrUnit in dbContext.Storage_ConstructionUnit
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
        }

		private static async Task<List<TransportOnFleetWithRegions>> GetTransportsOnFleetsAsync(ApplicationContext dbContext)
        {
            return await (from deliveryRegion in dbContext.DeliveryRegion
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
                 RegionIds = g.Select(r => r.RegionId).ToArray()
             })
             .ToListAsync();
        }

        private static Dictionary<int, List<ConstructionUnitSupplemented>> GetMaterialsSetsWithConstructionTypes(List<MaterialSet> materialSets, ApplicationContext dbContext, int constructionTypeId)
        {
            return (from mSet in materialSets
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
        }

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
            List<TransportOnFleetWithRegions> transportsOnFleetsAll = await GetTransportsOnFleetsAsync(dbContext);
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
            AddDeliveryVariantsForAirTransport(data, objectsToDeliver, transportsOnFleets, filterMethod);
            SortCostAndTimeListByFilterMethod(DeliveryVariants, filterMethod);
        }

		static void AddDeliveryVariantsForGroundTransport(AlgorithmData data, Objects objectsToDeliver, List<TransportOnFleetWithRegions> transportsOnFleets, FilterMethod filterMethod)
        {
            Dictionary<int, decimal?> storageToCertainObjectsDistances = GetStorageToCertainObjectsDistances(data.storageToObjectsDistances, objectsToDeliver);

            //цикл, в котором рассчитываются стоимости доставок от складов до объекта
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
                        //доставляет ли транспорт в этот регион?
                        if (transport.RegionIds.Contains(storage.RegionId) && transport.RegionIds.Contains(objectsToDeliver!.RegionId))
                        {
                            decimal distance = data.storageToTransportFleetDistances.FirstOrDefault(d => d.TransportFleetId == transportFleetId && d.StorageId == storageId)!.Distance + storageToCertainObjectsDistances[storageId] ?? decimal.MaxValue;

                            DeliveryVariants.Add(new DeliveryParamsUnit
                            {
                                TransportFleetId = transportFleetId,
                                TransportOnFleet = transport.TransportOnFleet,
                                StorageId = storageId,
                                Storage = storage,
                                Cost = (decimal)transport.TransportOnFleet!.CoefficientValue * distance,
                                DeliveryTime = distance / (decimal)transport.TransportOnFleet!.AverageSpeed,
                                Distance = distance
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
                            .CompareTo(t2.TransportOnFleet.CoefficientValue / t2.TransportOnFleet.AverageSpeed)
                        );
					break;
			}
        }

		static List<TransportOnFleetWithRegions> GetTransportToDeliverFromFleet(List<TransportOnFleetWithRegions> transportsOnFleets, Objects objectsToDeliver, TransportTypeValue transportType) =>
            transportsOnFleets
				.Where(t => t.RegionIds.Contains(objectsToDeliver.RegionId) && t.TransportOnFleet.TransportTypeId == (int)transportType)
				.ToList();

		static List<TransportOnFleetWithRegions> GetFirstTransportFromFleets(List<TransportOnFleetWithRegions> transportsOnFleets) =>
            transportsOnFleets.DistinctBy(t => t.TransportOnFleet.TransportFleet.TransportFleetId).ToList();

        static void AddDeliveryVariantsForAirTransport(AlgorithmData data, Objects objectsToDeliver, List<TransportOnFleetWithRegions> transportsOnFleets, FilterMethod filterMethod)
        {
            //взять воздушный транспорт, который может доставить до объекта

            List<TransportOnFleetWithRegions> airTransports = GetTransportToDeliverFromFleet(transportsOnFleets, objectsToDeliver, TransportTypeValue.Air);

            //взять парки, где он есть этот воздушный транспорт

            var transports = GetFirstTransportFromFleets(airTransports);

            //взять наземный транспорт который может доставить груз со склада до парка транспорта
            //
            //сортировка по соответствующему критерию
            //составление вариантов
        }



        static Dictionary<int, decimal?> GetStorageToCertainObjectsDistances(List<StorageToObjectsDistance> storageToObjectsDistances, Objects? objectsToDeliver)
        {
            return storageToObjectsDistances
                .Where(d => d.ObjectsId == objectsToDeliver!.ObjectsId)
                .ToDictionary(d => d.StorageId, d => d.Distance);
        }

        static List<TransportOnFleetWithRegions> FilterTransportFromTransportFleetByType(List<TransportOnFleetWithRegions> transportsOnFleets, int transportFleetId, TransportTypeValue transportTypeValue)
        {
            return transportsOnFleets
            .Where(
                t => t.TransportOnFleet.TransportFleet.TransportFleetId == transportFleetId
                && t.TransportOnFleet.TransportTypeId == (int)transportTypeValue
            ).ToList();
        }

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

            //цикл, в котором создаются варианты заказа для каждого сооружения
            foreach (var constructionOption in constructionOptions)
            {
                (Objects? objectsToDeliver, int constructionTypeId) = constructions
                    .Where(c => c.ConstructionId == constructionOption.ConstructionId)
                    .Select(c => (c.Object, c.ConstructionTypeId))
                    .FirstOrDefault();

                FillDeliveryVariants(dataTuple, constructionOption, objectsToDeliver);
                //формируем матрицу Storage_ConstructionUnits
                //достаём все элементы, которые могут содержаться в сооружении (то есть для начала берём наборы для сооружения)

                //берём наборы для конкретного сооружения из заказа и добавляем к ним тип материала
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
                    MaterialVariant?[,] storageMaterialMatrix = new MaterialVariant?[uniqueStorageIds.Length, uniqueConstructionUnitIds.Length];
                    MaterialVariant? materialVariant = null;

                    for (int storageId = 0; storageId < uniqueStorageIds.Length; storageId++)
                        for (int materialId = 0; materialId < uniqueConstructionUnitIds.Length; materialId++)
                        {
                            var constructionUnitFromStorage = constructionUnitsFromStorage
                                .FirstOrDefault(cUnit =>
                                cUnit!.ConstructionUnitId == uniqueConstructionUnitIds[materialId]
                                && cUnit.StorageId == uniqueStorageIds[storageId], null);

                            if (constructionUnitFromStorage is not null)
                                materialVariant = new MaterialVariant
                                {
                                    PricePerUnit = constructionUnitFromStorage.PricePerUnit,
                                    Amount = (decimal)constructionUnits[materialId].Amount,
                                };
                            storageMaterialMatrix[storageId, materialId] = materialVariant;
                        }

                    var orderVariants = CalculateOrderVariants(storageMaterialMatrix, uniqueStorageIds);
                    SortCostAndTimeListByFilterMethod(orderVariants, constructionOption.Filter.FilterMethod);

                    AlgorithmResponse response = GetOrderVariantsWithInfo(orderVariants, dataTuple, constructionUnits, manufaturerNames: GetManufacturersNamesByStorageIds(uniqueStorageIds, dbContext, dataTuple));
                }
            }
        }

        static AlgorithmResponse GetOrderVariantsWithInfo(List<ShortOrderVariant> orderVariants, AlgorithmData data, List<ConstructionUnitSupplemented> constructionUnits, Dictionary<int, string> manufaturerNames)
        {
            AlgorithmResponse response = new AlgorithmResponse();
            for (int i = 0; i < orderVariants.Count; i++)
            {
                var orderVariant = orderVariants[i];

                OrderVariant filledOrderVariant = new(new OrderResult(orderVariant.Cost, orderVariant.DeliveryTime));

                for (int constrUnitIndex = 0; constrUnitIndex < constructionUnits.Count; constrUnitIndex++)
                {
                    var deliveryVariant = orderVariant.DeliveryVariants[constrUnitIndex];

                    ConstructionUnitSupplemented constructionUnit = constructionUnits[constrUnitIndex];
                    Storage storage = data.storages.FirstOrDefault(s => s.StorageId == orderVariant.StorageIdsForMaterials[constrUnitIndex])!;

                    filledOrderVariant.BuildInfo.Add(new(constructionUnit.Name, constructionUnit.TypeName, constructionUnit.MeasureUnitName, constructionUnit.Amount));
                    filledOrderVariant.ProductionInfo.Add(new(manufaturerNames[storage.StorageId], storage.Name, storage.Address, orderVariant.MaterialPricesPerUnit[constrUnitIndex], orderVariant.MaterialCosts[constrUnitIndex]));
                    filledOrderVariant.LogisticInfo.Add(new(deliveryVariant));
                }
                response.OrderVariants.Add(filledOrderVariant);
            }
            return response;
        }

        static Dictionary<int, string> GetManufacturersNamesByStorageIds(int[] storageIds, ApplicationContext dbContext, AlgorithmData data)
        {
            return data.storages
                .Where(s => storageIds.Contains(s.StorageId))
                .Join(dbContext.Manufacturer,
                s => s.ManufacturerId,
                m => m.ManufacturerId,
                (s, m) => new { s.StorageId, ManufacturerName = m.Name })
                .ToDictionary(o => o.StorageId, o => o.ManufacturerName);
        }



        static List<ShortOrderVariant> CalculateOrderVariants(MaterialVariant?[,] storageMaterialMatrix, int[] storageIds)
        {
            //алгоритм для перебора, к сожалению, всех вариантов
            int storagesCount = storageMaterialMatrix.GetLength(0);
            int materialsCount = storageMaterialMatrix.GetLength(1);

            int[] storageIndicies = new int[materialsCount];
            List<ShortOrderVariant> orderVariants = new();

            while (storageIndicies.Last() < storagesCount)
            {
                bool variantIsValid = true;
                ShortOrderVariant orderVariant = new ShortOrderVariant(storageIndicies, new DeliveryParamsUnit[materialsCount], new decimal[materialsCount], new decimal[materialsCount], 0, 0);

                for (int materialIndex = 0; materialIndex < storageIndicies.Length; materialIndex++)
                {
                    MaterialVariant? materialVariant = storageMaterialMatrix[storageIndicies[materialIndex], materialIndex];

                    if (materialVariant is null)
                    {
                        variantIsValid = false;
                        break;
                    }

                    orderVariant.MaterialCosts[materialIndex] = materialVariant.Cost;
                    orderVariant.MaterialPricesPerUnit[materialIndex] = materialVariant.PricePerUnit;

                    orderVariant.Cost += materialVariant.Cost;

                    //TODO: переработать систему индексов или добавить преобразование!!!!
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
