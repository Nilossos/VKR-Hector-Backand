using NpgsqlTypes;

namespace Backand.AlgorithmEntities
{
	public readonly struct MissingDistance
	{
		public int Index1 { get; init; }
		public int Index2 { get; init; }
		public NpgsqlPoint Coordinates1 { get; init; }
		public NpgsqlPoint Coordinates2 { get; init; }
	}
}
