namespace Backand.FrontendEntities.AlgorithmResponse
{
	public class OrderResult
	{
		public decimal TotalCost { get; set; }
		public decimal DeliveryTime { get; set; }

		public OrderResult(decimal totalCost, decimal deliveryTime)
		{
			TotalCost = totalCost;
			DeliveryTime = deliveryTime;
		}
	}
}
