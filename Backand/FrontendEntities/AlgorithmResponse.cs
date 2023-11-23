using Backand.DbEntities;

namespace Backand.FrontendEntities
{
	public class BuildInfo
	{
		public string ConstructionUnitName { get; set; }
		public string ConstructionUnitType { get; set; }
		public string MeasuringUnit { get; set; }
		public double Amount { get; set; }
	}

	public class ProductionInfo
	{
		public string ManufacturerName { get; set; }
		public string StorageName { get; set;}
		public string StorageAddress { get; set; }

		// UNDONE: кусочек пустого интерфейса пользователя

		public decimal PricePerUnit { get; set; }
		public decimal PurchasePrice { get; set; }

	}

	public class LogisticInfo
	{
		public string LogisticCompanyName { get; set; }
		public string TransportFleetName { get; set; }
		public string TransportFleetAddress { get; set; }
		public string TransportName { get; set; }
		public string TransportTypeName { get; set; }
		public string TransportModeName { get; set; }
		public int CoefficientType { get; set; }
		public string CoefficientValue { get; set; }
		public int DeliveryDistance { get; set; }
		public int DeliveryTime { get; set; }
		public decimal DeliveryCost { get; set; }
	}

	public class OrderResult
	{
		public decimal TotalCosts { get; set; }
		public DateTime DeliveryTime { get; set; }
	}

	public class AlgorithmResponse
	{
		public BuildInfo BuildInfo { get; set; }
		public ProductionInfo ProductionInfo { get; set; }
		public List<LogisticInfo> LogisticInfos { get; set; }
		public OrderResult OrderResult { get; set; }
	}
}
