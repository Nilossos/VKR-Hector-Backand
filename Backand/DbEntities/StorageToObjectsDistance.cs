using Microsoft.EntityFrameworkCore;

namespace Backand.DbEntities
{
    [PrimaryKey(nameof(StorageId), nameof(ObjectsId))]
    public class StorageToObjectsDistance
	{
		public int StorageId { get; init; }

		public int ObjectsId { get; init; }

		public decimal? Distance { get; init; }
	}
}
