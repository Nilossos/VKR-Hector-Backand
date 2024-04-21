using Backand.DbEntites;

namespace Backand.AlgorithmEntities
{
	public class StorageMaterial
	{
		public int StorageId { get; init; }
		public string StorageName { get; init; } = null!;
		public int ConstructionUnitId { get; init; }
		public BuildWay ConstructionUnitTypeId { get; init; }
		public int ManufacturerId { get; init; }
		public decimal PricePerUnit { get; init; }
	}
}
