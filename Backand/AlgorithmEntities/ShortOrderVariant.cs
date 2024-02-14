namespace Backand.AlgorithmEntities
{
	public class ShortOrderVariant : ICostAndTime
	{
		public int[] StorageIdsForMaterials { get; set; }
		public DeliveryParamsUnit[] DeliveryVariants { get; set; }
		public decimal[] MaterialCosts { get; set; }
		public decimal[] MaterialPricesPerUnit { get; set; }
		public decimal Cost { get; set; }
		public decimal DeliveryTime { get; set; }

		public ShortOrderVariant(int[] storageIdsForMaterials, DeliveryParamsUnit[] deliveryParams, decimal[] materialCosts, decimal[] materialPricesPerUnit, decimal cost, decimal deliveryTime)
		{
			StorageIdsForMaterials = storageIdsForMaterials!;
			DeliveryVariants = deliveryParams;
			MaterialCosts = materialCosts;
			MaterialPricesPerUnit = materialPricesPerUnit;
			Cost = cost;
			DeliveryTime = deliveryTime;
		}
	}
}
