﻿using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.DbEntities.ConstructionSpace;
using Backand.FrontendEntities.AlgorithmResponse;
using Backand.FrontendEntities.Links;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class InnerDataRequests
	{
		internal static List<TransportOnFleetWithRegions> GetTransportToDeliverFromFleet(List<TransportOnFleetWithRegions> transportsOnFleets, DbEntities.ObjectEntity objectsToDeliver, TransportTypeValue transportType) =>
			transportsOnFleets
				.Where(t => t.RegionIds.Contains(objectsToDeliver.RegionId) && t.TransportOnFleet.TransportTypeId == (int)transportType)
				.ToList();

		internal static List<TransportOnFleetWithRegions> GetGroundTransportToDeliverInAnyRegion(List<TransportOnFleetWithRegions> transportsOnFleets, int[] regionIds) =>
			transportsOnFleets.Where(t => t.TransportOnFleet.TransportTypeId == (int)TransportTypeValue.Ground && t.RegionIds.Any(r => regionIds.Contains(r!))).ToList();

		internal static TransportOnFleetWithRegions? GetFirstTransportToDeliverInRegion(List<TransportOnFleetWithRegions> transportsOnFleets, int regionId) =>
			transportsOnFleets.FirstOrDefault(t => t.RegionIds.Contains(regionId));

		internal static List<TransportOnFleetWithRegions> GetFirstTransportFromFleets(List<TransportOnFleetWithRegions> transportsOnFleets) =>
			transportsOnFleets.DistinctBy(t => t.TransportOnFleet.TransportFleet.TransportFleetId).ToList();

		internal static Dictionary<int, decimal?> GetStorageToCertainObjectsDistances(List<StorageToObjectsDistance> storageToObjectsDistances, DbEntities.ObjectEntity? objectsToDeliver) =>
			storageToObjectsDistances
				.Where(d => d.ObjectsId == objectsToDeliver!.ObjectsId)
				.ToDictionary(d => d.StorageId, d => d.Distance);

		internal static decimal? GetStorageToTransportFleetDistance(List<StorageToTransportFleetDistance> storageToTransportFleetDistances, int storageId, int transportFleetId)
		{
			StorageToTransportFleetDistance? distanceObject = storageToTransportFleetDistances
				.FirstOrDefault(d => d.TransportFleetId == transportFleetId && d.StorageId == storageId);

			if (distanceObject is null)
				return null;
			else
				return distanceObject.Distance;
		}

		internal static decimal? GetTransportFleetToObjectsDistance(List<TransportFleetToObjectsDistance> transportFleetToObjectsDistances, int transportFleetId, int objectsId)
		{
			TransportFleetToObjectsDistance? distanceObject = transportFleetToObjectsDistances
				.FirstOrDefault(d => d.TransportFleetId == transportFleetId && d.ObjectsId == objectsId);

			if (distanceObject is null) 
				return null;
			else
				return distanceObject.Distance;
		}

		public static async Task<Construction?> GetConstructionById(ApplicationContext context, int constructionId) => await
		context
			.Construction
			.Include(construction => construction.Object)
				.ThenInclude(objects => objects!.Mine)
					.ThenInclude(mine => mine!.Subsidiary)
			.Include(construction => construction.ConstructionType)
			.FirstOrDefaultAsync(construction => construction.ConstructionId == constructionId);

		internal static async Task<Order> GetOrderVariantsWithInfoAsync(List<ShortOrderVariant> orderVariants, AlgorithmData data, List<ConstructionUnitSupplemented> constructionUnits, Dictionary<int, Manufacturer> storageManufaturers, bool isAssemblyBuildRequired, int constructionId, ApplicationContext context)
		{
			Order order = new(); // { IsAssemblyBuildRequired = isAssemblyBuildRequired };

			var construction = await GetConstructionById(context, constructionId) ??
							   throw new NullReferenceException();

			order.Construction = new EntityLink { Id = constructionId, Name = construction.ConstructionName };
			order.Objects = new EntityLink { Id = construction.ObjectId, Name = construction.Object.Name! };
			order.Mine = new EntityLink { Id = construction.Object.MineId, Name = construction.Object.Mine!.Name };
			order.Subsidiary = new EntityLink { Id = construction.Object.Mine.SubsidiaryId ?? 0, Name = construction.Object.Mine.Subsidiary.Name };

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
					ProductionInfo productionInfo = new(storageManufaturers[storage.StorageId].Name, storage.Name, storage.Address, orderVariant.MaterialPricesPerUnit[constrUnitIndex], orderVariant.MaterialCosts[constrUnitIndex]);
					List<LogisticInfo> logisticInfo = LogisticInfo.CreateLogisticInfoList(deliveryVariant.DeliveryRouteParams);

					materialOrderVariants.Add(new(buildInfo, productionInfo, logisticInfo));
				}
				OrderVariant filledOrderVariant = new(materialOrderVariants, orderResult);

				order.Variants.Add(filledOrderVariant);
			}
			return order;
		}
	}
}
