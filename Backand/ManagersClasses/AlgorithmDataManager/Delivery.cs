using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;
using static Backand.ManagersClasses.AlgorithmDataManager.DataFiltering;
using static Backand.ManagersClasses.AlgorithmDataManager.InnerDataRequests;
using static Backand.ManagersClasses.AlgorithmDataManager.DataSorting;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class Delivery
	{
		internal static List<DeliveryVariant> DeliveryVariants { get; set; } = new();

		internal static void FillDeliveryVariants(AlgorithmData data, ConstructionWithFilters constructionOption, DbEntities.ObjectEntity objectToDeliver)
		{
			DeliveryVariants.Clear();

			List<TransportOnFleetWithRegions> transportsOnFleets = FilterFleetsByLogisticCompanies(data.transportsOnFleetsAll, constructionOption);
			TargetMark targetMark = constructionOption.Filter.TargetMark;
			var transportType = constructionOption.Filter.TransportTypeIds.Cast<TransportTypeValue>();

			if (transportType.Contains(TransportTypeValue.Ground))
				AddDeliveryVariantsForGroundTransport(data, objectToDeliver, transportsOnFleets, targetMark);

			if (transportType.Contains(TransportTypeValue.Air))
				AddDeliveryVariantsForAirTransport(data, objectToDeliver, transportsOnFleets, targetMark);

			SortCostAndTimeListByFilterMethod(DeliveryVariants, targetMark);
		}

		static void AddDeliveryVariantsForGroundTransport(AlgorithmData data, DbEntities.ObjectEntity objectToDeliver, List<TransportOnFleetWithRegions> transportsOnFleets, TargetMark filterMethod)
		{
			Dictionary<int, decimal?> storageToCertainObjectDistances = GetStorageToCertainObjectDistances(data.storageToObjectDistances, objectToDeliver);

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
						if (transport.RegionIds.Contains((int)storage.RegionId!) && transport.RegionIds.Contains(objectToDeliver!.RegionId))
						{
							decimal distance = (decimal)(data.storageToTransportFleetDistances.FirstOrDefault(d => d.TransportFleetId == transportFleetId && d.StorageId == storageId)!.Distance + storageToCertainObjectDistances[storageId])!;
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

		static void AddDeliveryVariantsForAirTransport(AlgorithmData data, ObjectEntity objectToDeliver, List<TransportOnFleetWithRegions> transportsOnFleets, TargetMark filterMethod)
		{
			List<TransportOnFleetWithRegions> airTransport = GetTransportToDeliverFromFleet(transportsOnFleets, objectToDeliver, TransportTypeValue.Air);
			SortTransportsOnFleetByFilter(airTransport, filterMethod);
			var airTransportFiltered = GetFirstTransportFromFleets(airTransport);

			int[] regionIds = airTransportFiltered.Select(t => t.TransportOnFleet.TransportFleet.RegionId).ToArray();
			var groundTransport = GetGroundTransportToDeliverInAnyRegion(airTransportFiltered, regionIds);
			SortTransportsOnFleetByFilter(groundTransport, filterMethod);
			var groundTransportFiltered = GetFirstTransportFromFleets(groundTransport);

			GetGroundAirTransportVariations(data, airTransportFiltered, groundTransportFiltered, objectToDeliver.ObjectId);
		}

		static void GetGroundAirTransportVariations(AlgorithmData data, List<TransportOnFleetWithRegions> airTransports, List<TransportOnFleetWithRegions> groundTransports, int objectToDeliverId)
		{

			foreach (var storage in data.storages)
				foreach (var airTransport in airTransports)
				{
					int airTransportFleetId = airTransport.TransportOnFleet.TransportFleet.TransportFleetId;
					decimal? distanceToAirTransport = GetStorageToTransportFleetDistance(data.storageToTransportFleetDistances, storage.StorageId, airTransportFleetId);

					if (distanceToAirTransport is null) continue;

					var airRegionId = airTransport.TransportOnFleet.TransportFleet.RegionId;
					var groundTransport = GetFirstTransportToDeliverInRegion(groundTransports, airRegionId);
					if (groundTransport is null) continue;

					int groundTransportFleetId = groundTransport.TransportOnFleet.TransportFleet.TransportFleetId;

					decimal? groundDistance = GetStorageToTransportFleetDistance(data.storageToTransportFleetDistances, storage.StorageId, groundTransportFleetId) + distanceToAirTransport;
					if (groundDistance is null) continue;
					decimal groundCost = (decimal)groundTransport.TransportOnFleet!.CoefficientValue * (decimal)groundDistance;
					decimal groundDeliveryTime = (decimal)groundDistance / (decimal)groundTransport.TransportOnFleet.AverageSpeed;

					decimal? airDistance = GetTransportFleetToObjectDistance(data.transportFleetToObjectDistances, airTransportFleetId, objectToDeliverId);
					if (airDistance is null) continue;
					decimal airCost = (decimal)airTransport.TransportOnFleet!.CoefficientValue * (decimal)airDistance;
					decimal airDeliveryTime = (decimal)airDistance / (decimal)airTransport.TransportOnFleet.AverageSpeed;

					DeliveryVariants.Add(new DeliveryVariant()
					{
						StorageId = storage.StorageId,
						Storage = storage,
						DeliveryRouteParams = new List<DeliveryRouteParams>()
						{
							new DeliveryRouteParams()
							{
								TransportFleetId = groundTransportFleetId,
								TransportOnFleet = groundTransport.TransportOnFleet,
								Cost = groundCost,
								DeliveryTime = groundDeliveryTime,
								Distance = (decimal)groundDistance
							},
							new DeliveryRouteParams()
							{
								TransportFleetId = airTransportFleetId,
								TransportOnFleet = airTransport.TransportOnFleet,
								Cost = airCost,
								DeliveryTime = airDeliveryTime,
								Distance = (decimal)airDistance
							}
						},
						Cost = groundCost + airCost,
						DeliveryTime = groundDeliveryTime + airDeliveryTime
					});
				}
		}
	}
}
