using Backand.DbEntities;

namespace Backand.AlgorithmEntities
{
	public class TransportOnFleet
	{
		//public int TransportFleet_TransportId { get; init; }
		public TransportFleet TransportFleet { get; init; }
		public int TransportId { get; init; }
		public string TransportName { get; init; } = null!;
		public int CoefficientTypeId { get; init; }
		public string CoefficientTypeName { get; init; } = null!;
		public float CoefficientValue { get; init; }
		public int TransportTypeId { get; init; }
		public string TransportTypeName { get; init; } = null!;
		public string TransportModeName { get; init; } = null!;
		public double AverageSpeed { get; init; }
		public int CompanyId { get; set; }
		public string CompanyName { get; init; } = null!;
		public int CompanyTypeId { get; init; }
	}
}
