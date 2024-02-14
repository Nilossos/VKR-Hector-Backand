using Backand.DbEntities;

namespace Backand.AlgorithmEntities
{
	public class DeliveryParamsUnit : ICostAndTime
	{
		public int TransportFleetId { get; set; }
		public TransportOnFleet? TransportOnFleet { get; set; }
		public int StorageId { get; set; }
		public Storage? Storage { get; set; }
		public decimal Cost { get; set; }
		public decimal DeliveryTime { get; set; }
		public decimal Distance { get; set; }
	}
}
