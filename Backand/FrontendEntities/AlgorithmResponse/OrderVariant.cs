namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class OrderVariant
	{
		public IEnumerable<MaterialOrderVariant> MaterialOrderVariants { get; set; }

		public OrderResult OrderResult => new()
		{
			DeliveryTime = MaterialOrderVariants.Sum(variant => variant.LogisticInfos.Sum(info => info.DeliveryTime)),
			TotalCost = MaterialOrderVariants.Sum(variant => variant.ProductionInfo.PurchasePrice +
			                                                 variant.LogisticInfos.Sum(info => info.DeliveryCost))
		};

		public OrderVariant(IEnumerable<MaterialOrderVariant> materialOrderVariants, OrderResult orderResult)
		{
			MaterialOrderVariants = materialOrderVariants;
			//OrderResult = orderResult;
		}

		public OrderVariant() { }
		
		public bool IsAssemblyBuildRequired { get; set; }
	}
}
