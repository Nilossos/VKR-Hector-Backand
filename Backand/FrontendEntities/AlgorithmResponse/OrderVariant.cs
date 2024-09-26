namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class OrderVariant
	{
		public IEnumerable<MaterialOrderVariant> MaterialOrderVariants { get; set; }

		public OrderResult OrderResult => new()
		{
			DeliveryTime = MaterialOrderVariants.Max(variant => variant.LogisticInfos.Sum(info => info.DeliveryTime)),
			TotalCost = MaterialOrderVariants.Sum(variant => variant.ProductionInfo.PurchasePrice + variant.LogisticInfos.Sum(info => info.DeliveryCost))
		};

		public OrderVariant(IEnumerable<MaterialOrderVariant> materialOrderVariants, OrderResult orderResult)
		{
			MaterialOrderVariants = materialOrderVariants;
			//OrderResult = orderResult;
		}

		public OrderVariant() { }
		
		public bool IsAssemblyBuildRequired { get; set; }
        public string ConstructionUnitType { get; set; }
        public string TransportTypeName { get; set; }

        public string GetUniqueTransportTypes(List<MaterialOrderVariant> materialOrderVariants)
        {
            HashSet<string> transportTypes = new HashSet<string>();

            foreach (MaterialOrderVariant variant in materialOrderVariants)
            {
                foreach (LogisticInfo logisticInfo in variant.LogisticInfos)
                {
                    transportTypes.Add(logisticInfo.TransportTypeName);
                }
            }

            return string.Join(", ", transportTypes);
        }
    }
}
