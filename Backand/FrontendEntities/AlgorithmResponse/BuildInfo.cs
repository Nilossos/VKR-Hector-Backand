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
}
