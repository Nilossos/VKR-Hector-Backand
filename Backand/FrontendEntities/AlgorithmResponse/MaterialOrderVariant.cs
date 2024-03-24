namespace Backand.FrontendEntities.AlgorithmResponse
{
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
}
