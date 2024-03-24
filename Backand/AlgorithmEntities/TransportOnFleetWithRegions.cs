namespace Backand.AlgorithmEntities
{
	public class TransportOnFleetWithRegions
	{
		public TransportOnFleet TransportOnFleet { get; init; } = null!;
		public int[] RegionIds { get; init; } = null!;
	}
}
