using Backand.FrontendEntities.Links;

namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class Order
	{
		public EntityLink Construction { get; set; } = null!;
		public EntityLink Objects { get; set; } = null!;
		public EntityLink Mine { get; set; } = null!;
		public EntityLink Subsidiary { get; set; } = null!;
		public List<OrderVariant> Variants { get; init; } = new();
		public bool IsAssemblyBuildRequired { get; init; }
	}
}
