using Microsoft.EntityFrameworkCore;

namespace Backand.DbEntities
{
	// TODO: Перевести StorageToObject на рельсы Lazy Loading;
    [PrimaryKey(nameof(StorageId), nameof(ObjectId))]
    public class StorageToObjectDistance
	{
		public int StorageId { get; init; }

		public int ObjectId { get; init; }

		public decimal? Distance { get; init; }

		public virtual Storage Storage { get; set; } = null!;

		public virtual ObjectEntity Object { get; set; } = null!;
	}
}
