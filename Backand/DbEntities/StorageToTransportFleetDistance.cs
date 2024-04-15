using Microsoft.EntityFrameworkCore;

namespace Backand.DbEntities
{
	// TODO: Перевести StorageToTransportFleetDistance на рельсы Lazy Loading;
    [PrimaryKey(nameof(StorageId), nameof(TransportFleetId))]
    public class StorageToTransportFleetDistance
	{
		public int StorageId { get; init; }

		public int TransportFleetId { get; init; }

		public decimal? Distance { get; init; }
		
		public virtual Storage Storage { get; set; } = null!;

		public virtual TransportFleet TransportFleet { get; set; } = null!;
	}
}
