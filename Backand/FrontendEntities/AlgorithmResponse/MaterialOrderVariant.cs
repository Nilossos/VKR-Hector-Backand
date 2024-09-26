namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class MaterialOrderVariant
	{
		public BuildInfo BuildInfo { get; init; }
		public ProductionInfo ProductionInfo { get; init; }
		public List<LogisticInfo> LogisticInfos { get; init; }
        public decimal TotalDeliveryCost => (LogisticInfos?.Sum(info => info.DeliveryCost) ?? 0) + (ProductionInfo?.PurchasePrice ?? 0);
        public decimal TotalDeliveryTime => LogisticInfos?.Sum(info => info.DeliveryTime) ?? 0;

        public MaterialOrderVariant(BuildInfo buildInfo, ProductionInfo productionInfo, List<LogisticInfo> logisticInfos)
		{
			BuildInfo = buildInfo;
			ProductionInfo = productionInfo;
			LogisticInfos = logisticInfos;
		}

		public MaterialOrderVariant() { }
	}
}
