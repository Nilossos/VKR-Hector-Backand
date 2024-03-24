namespace Backand.FrontendEntities.AlgorithmResponse
{
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
}
