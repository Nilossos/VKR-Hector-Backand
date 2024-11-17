using Backand.DbEntities.ConstructionSpace;
using Backand.FrontendEntities.Links;

namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class Order
	{
		public Construction Construction { get; set; } = null!;
		public EntityLink Object { get; set; } = null!;
		public EntityLink Mine { get; set; } = null!;
		public EntityLink Subsidiary { get; set; } = null!;
		public List<OrderVariant> Variants { get; init; } = new();
		public bool IsAssemblyBuildRequired { get; init; }
	}
}
