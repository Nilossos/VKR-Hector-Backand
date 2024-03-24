namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class OrderVariant
	{
		public IEnumerable<MaterialOrderVariant> MaterialOrderVariants { get; set; }
		public OrderResult OrderResult { get; set; }

		public OrderVariant(IEnumerable<MaterialOrderVariant> materialOrderVariants, OrderResult orderResult)
		{
			MaterialOrderVariants = materialOrderVariants;
			OrderResult = orderResult;
		}
	}
}
