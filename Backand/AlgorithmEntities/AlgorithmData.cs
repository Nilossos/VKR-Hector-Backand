using Backand.DbEntities;
using Microsoft.IdentityModel.Tokens;

namespace Backand.AlgorithmEntities
{
    public static class AlgorithmData
    {
        public static int[,]? StorageTransportFleetDistances;
        public static int[,]? StorageObjectsDistances;
    }

    public struct DataElement
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public double Value { get; set; }
    }

    public class TransportOnFleet
    {

        public TransportOnFleet(int transportFleetId, int transportId, int coefficientTypeId, float coefficientValue, int transportTypeId, int averageSpeed, string transportTypeName, string transportName, int companyId, int companyTypeId)
        {
            TransportFleetId = transportFleetId;
            TransportId = transportId;
            CoefficientTypeId = coefficientTypeId;
            CoefficientValue = coefficientValue;
            TransportTypeId = transportTypeId;
            AverageSpeed = averageSpeed;
            TransportTypeName = transportTypeName;
            TransportName = transportName;
            CompanyId = companyId;
            CompanyTypeId = companyTypeId;
        }

        public int TransportFleetId { get; init; }

		public int TransportId { get; init; }

        public int CoefficientTypeId { get; init; }

        public float CoefficientValue { get; init; }

        public int TransportTypeId { get; init; }

        public double AverageSpeed { get; init; }

		public string? TransportTypeName { get; init; }

		public string? TransportName { get; init; }

        public int CompanyId { get; init; }

        public int CompanyTypeId { get; init; }
	}

    public class StorageMaterial
    {
        public int StorageId { get; init; }

        public string? StorageName { get; init; }

        public int ConstructionUnitId { get; init; }

        public int ConstructionUnitTypeId { get; init; }

        public int ManufacturerId { get; init; }

        public decimal PricePerUnit { get; init; }
    }
}
