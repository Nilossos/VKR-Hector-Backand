using Backand.AlgorithmEntities;

namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class LogisticInfo
	{
		public string LogisticCompanyName { get; set; }
		public string TransportFleetName { get; set; }
		public string TransportFleetAddress { get; set; }
		public string TransportName { get; set; }
		public string TransportTypeName { get; set; }
		public string TransportModeName { get; set; }
		public string CoefficientTypeName { get; set; }
		public float? CoefficientValue { get; set; }
		public decimal DeliveryDistance { get; set; }
		public decimal DeliveryTime { get; set; }
		public decimal DeliveryCost { get; set; }

		public LogisticInfo(string logisticCompanyName, string transportFleetName, string transportFleetAddress, string transportName, string transportTypeName, string transportModeName, string coefficientTypeName, float coefficientValue, decimal deliveryDistance, decimal deliveryTime, decimal deliveryCost)
		{
			LogisticCompanyName = logisticCompanyName;
			TransportFleetName = transportFleetName;
			TransportFleetAddress = transportFleetAddress;
			TransportName = transportName;
			TransportTypeName = transportTypeName;
			TransportModeName = transportModeName;
			CoefficientTypeName = coefficientTypeName;
			CoefficientValue = coefficientValue;
			DeliveryDistance = deliveryDistance;
			DeliveryTime = deliveryTime;
			DeliveryCost = deliveryCost;
		}

		public LogisticInfo(DeliveryRouteParams deliveryVariant)
		{
			LogisticCompanyName = deliveryVariant.TransportOnFleet.CompanyName;
			TransportFleetName = deliveryVariant.TransportOnFleet.TransportFleet.Name;
			TransportFleetAddress = deliveryVariant.TransportOnFleet.TransportFleet.Address;
			TransportName = deliveryVariant.TransportOnFleet.TransportName;
			TransportTypeName = deliveryVariant.TransportOnFleet.TransportTypeName;
			TransportModeName = deliveryVariant.TransportOnFleet.TransportModeName;
			CoefficientTypeName = deliveryVariant.TransportOnFleet.CoefficientTypeName;
			CoefficientValue = deliveryVariant.TransportOnFleet.CoefficientValue;
			DeliveryDistance = deliveryVariant.Distance;
			DeliveryTime = deliveryVariant.DeliveryTime;
			DeliveryCost = deliveryVariant.Cost;
		}

		public static List<LogisticInfo> CreateLogisticInfoList(IEnumerable<DeliveryRouteParams> deliveryRoutes)
		{
			List<LogisticInfo> logisticInfos = new();
			foreach (var deliveryParams in deliveryRoutes)
				logisticInfos.Add(new LogisticInfo(deliveryParams));

			return logisticInfos;
		}
	}
}
