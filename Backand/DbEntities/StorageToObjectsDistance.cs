using Microsoft.EntityFrameworkCore;

namespace Backand.DbEntities
{
	// TODO: Перевести StorageToObjects на рельсы Lazy Loading;
    [PrimaryKey(nameof(StorageId), nameof(ObjectsId))]
    public class StorageToObjectsDistance
	{
		public int StorageId { get; init; }

		public int ObjectsId { get; init; }

		public decimal? Distance { get; init; }

		public virtual Storage Storage { get; set; } = null!;

		public virtual Objects Objects { get; set; } = null!;
	}
}
