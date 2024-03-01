using Backand.DbEntities;

namespace Backand.AlgorithmEntities
{
	public class DeliveryRouteParams : ICostAndTime
	{
		public int TransportFleetId { get; set; }
		public TransportOnFleet TransportOnFleet { get; set; } = null!;
		public decimal Cost { get; set; }
		public decimal DeliveryTime { get; set; }
		public decimal Distance { get; set; }
	}
}
