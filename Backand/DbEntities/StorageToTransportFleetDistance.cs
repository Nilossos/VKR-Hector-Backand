using Microsoft.EntityFrameworkCore;

namespace Backand.DbEntities
{
    [PrimaryKey(nameof(StorageId), nameof(TransportFleetId))]
    public class StorageToTransportFleetDistance
	{
		public int StorageId { get; init; }

		public int TransportFleetId { get; init; }

		public decimal? Distance { get; init; }
	}
}
