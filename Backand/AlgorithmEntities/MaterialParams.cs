namespace Backand.AlgorithmEntities
{
	public class MaterialParams
	{
		public decimal PricePerUnit { get; set; }
		public decimal Amount { get; set; }
		public decimal Cost => PricePerUnit * Amount;
	}
}
