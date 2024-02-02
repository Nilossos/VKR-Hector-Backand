namespace Backand.FrontendEntities
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
		public string StorageName { get; set;}
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
		public string? LogisticCompanyName { get; set; }
		public string? TransportFleetName { get; set; }
		public string? TransportFleetAddress { get; set; }
		public string? TransportName { get; set; }
		public string? TransportTypeName { get; set; }
		public string? TransportModeName { get; set; }
		public string CoefficientTypeName { get; set; }
		public float? CoefficientValue { get; set; }
		public decimal DeliveryDistance { get; set; }
		public decimal DeliveryTime { get; set; }
		public decimal DeliveryCost { get; set; }

		public LogisticInfo(string? logisticCompanyName, string? transportFleetName, string? transportFleetAddress, string? transportName, string? transportTypeName, string? transportModeName, string coefficientTypeName, float coefficientValue, decimal deliveryDistance, decimal deliveryTime, decimal deliveryCost)
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
		public List<BuildInfo> BuildInfo { get; set; } = new();
		public List<ProductionInfo> ProductionInfo { get; set; } = new();
		public List<LogisticInfo> LogisticInfo { get; set; } = new();
		public OrderResult OrderResult { get; set; }

		public OrderVariant(OrderResult orderResult) 
		{
			OrderResult = orderResult;
		}
	}

	public class AlgorithmResponse
	{
		public List<OrderVariant>? OrderVariants { get; set; }
	}
}
