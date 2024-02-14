using Backand.DbEntities;

namespace Backand.AlgorithmEntities
{
	public class ConstructionUnitSupplemented : ConstructionUnit
	{
		public string MeasureUnitName { get; init; } = null!;
		public string TypeName { get; init; } = null!;
		public double Amount { get; init; }
	}
}
