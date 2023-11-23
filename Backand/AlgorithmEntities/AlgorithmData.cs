using Backand.DbEntities;

namespace Backand.AlgorithmEntities
{
    public class AlgorithmData
    {
        //пока пусто
    }

    public class TransportOnFleet
    {
		public int TransportFleetId { get; init; }

		public int TransportId { get; init; }

		public int AverageSpeed { get; init; }

		public int CoefficientTypeId { get; init; }

		public float CoefficientValue { get; init; }

		public string? TransportName { get; init; }

		public int TransportType { get; init; }


	}
}
