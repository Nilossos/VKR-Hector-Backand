namespace Backand.AlgorithmEntities
{
	public class MaterialVariant
	{
		public decimal PricePerUnit { get; set; }
		public decimal Amount { get; set; }
		public decimal Cost { get => PricePerUnit * Amount; }
	}
}
