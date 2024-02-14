using NpgsqlTypes;

namespace Backand.AlgorithmEntities
{
	public readonly struct UnitIdWithCoordinates
	{
		public int Id { get; init; }
		public NpgsqlPoint Coordinates { get; init; }
		public UnitIdWithCoordinates(int id, NpgsqlPoint coordinates)
		{
			Id = id;
			Coordinates = coordinates;
		}
	}
}
