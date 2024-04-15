namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class Order
	{
		public int ConstructionId { get; set; }
		public string ConstructionName { get; set; }
		public int ObjectsId { get; set; }
		public string ConstructionType { get; set; }
		public int LinkId { get; set; }
		public string LinkName { get; set; }
		
		public List<OrderVariant> Variants { get; init; } = new();
    }
}
