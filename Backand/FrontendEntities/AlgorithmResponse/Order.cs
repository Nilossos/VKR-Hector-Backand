namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class Order
	{
		public List<OrderVariant> Variants { get; init; } = new();
		public bool IsAssemblyBuildRequired { get; init; }
    }
}
