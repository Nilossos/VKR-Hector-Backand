using Backand.DbEntities;

namespace Backand.AlgorithmEntities
{
	public class DeliveryVariant : ICostAndTime
	{
		public int StorageId { get; set; }
		public Storage Storage { get; set; } = null!;
		public IEnumerable<DeliveryRouteParams> DeliveryRouteParams { get; set; } = null!;
		public decimal Cost { get; set; }
		public decimal DeliveryTime { get; set; }
	}
}
