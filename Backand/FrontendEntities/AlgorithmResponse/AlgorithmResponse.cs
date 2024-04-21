using Backand.AlgorithmEntities;
using System.Security.Cryptography.X509Certificates;

namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class BuildInfo
	{
		public string ConstructionUnitName { get; set; }
		public string ConstructionUnitType { get; set; }
		public string MeasuringUnit { get; set; }
		public double Amount { get; set; }

		public BuildInfo(string constructionUnitName, string constructionUnitType, string measuringUnit, double amount)
		{
			ConstructionUnitName = constructionUnitName;
			ConstructionUnitType = constructionUnitType;
			MeasuringUnit = measuringUnit;
			Amount = amount;
		}
	}

	public class ProductionInfo
	{
		public string ManufacturerName { get; set; }
		public string StorageName { get; set; }
		public string StorageAddress { get; set; }
		public decimal PricePerUnit { get; set; }
		public decimal PurchasePrice { get; set; }

		public ProductionInfo(string manufacturerName, string storageName, string storageAddress, decimal pricePerUnit, decimal purchasePrice)
		{
			ManufacturerName = manufacturerName;
			StorageName = storageName;
			StorageAddress = storageAddress;
			PricePerUnit = pricePerUnit;
			PurchasePrice = purchasePrice;
		}
	}

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
			foreach(var deliveryParams in deliveryRoutes)
				logisticInfos.Add(new LogisticInfo(deliveryParams));

			return logisticInfos;
		}
	}

	public class OrderResult
	{
		public decimal TotalCost { get; set; }
		public decimal DeliveryTime { get; set; }

		public OrderResult(decimal totalCost, decimal deliveryTime)
		{
			TotalCost = totalCost;
			DeliveryTime = deliveryTime;
		}
	}

	public class OrderVariant
	{
		public IEnumerable<MaterialOrderVariant> MaterialOrderVariants { get; set; }
		public OrderResult OrderResult { get; set; }

		public OrderVariant(IEnumerable<MaterialOrderVariant> materialOrderVariants, OrderResult orderResult)
		{
			MaterialOrderVariants = materialOrderVariants;
			OrderResult = orderResult;
		}
	}

	public class MaterialOrderVariant
	{
		public BuildInfo BuildInfo { get; init; }
		public ProductionInfo ProductionInfo { get; init; }
		public List<LogisticInfo> LogisticInfos { get; init; }
		public MaterialOrderVariant(BuildInfo buildInfo, ProductionInfo productionInfo, List<LogisticInfo> logisticInfos)
		{
			BuildInfo = buildInfo;
			ProductionInfo = productionInfo;
			LogisticInfos = logisticInfos;
		}
	}

	public class Order
	{
		public List<OrderVariant> Variants { get; init; } = null!;
	}

	public class AlgorithmResponse
	{
		public List<Order> Orders { get; init; } = null!;
	}
}
