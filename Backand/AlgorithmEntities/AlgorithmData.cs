using Backand.DbEntities;
using NpgsqlTypes;

namespace Backand.AlgorithmEntities
{
	public record AlgorithmData
	(
        List<Construction> constructions,
        List<Storage> storages,
        List<TransportFleet> transportFleets,
        List<TransportOnFleetWithRegions> transportsOnFleetsAll,
        List<MaterialSet> materialSets,
        List<StorageMaterial> storagesMaterialsAll,
        List<StorageToObjectsDistance> storageToObjectsDistances,
        List<StorageToTransportFleetDistance> storageToTransportFleetDistances
        //List<TransportFleetToObjectsDistance> transportFleetToObjectsDistances
    );

    //public record AlgorithmData
    //{
    //    public List<Construction> constructions { get; init; }
    //    public List<Storage> storages { get; init; }
    //    public List<TransportFleet> transportFleets { get; init; }
    //    public List<TransportOnFleetWithRegions> transportsOnFleetsAll { get; init; }
    //    public List<MaterialSet> materialSets { get; init; }
    //    public List<StorageMaterial> storagesMaterialsAll { get; init; }
    //};

	public class MaterialVariant
	{
		public decimal PricePerUnit { get; set; }
		public decimal Amount { get; set; }
		public decimal Cost { get => PricePerUnit * Amount; }
	}

    public readonly struct MissingDistance
	{
		public int Index1 { get; init; }
		public int Index2 { get; init; }
		public NpgsqlPoint Coordinates1 { get; init; }
		public NpgsqlPoint Coordinates2 { get; init; }
	}

	public readonly struct UnitIdWithCoordinates
	{
		public int Id { get; init; }
		public NpgsqlPoint Coordinates { get; init; }
	}

	public class ShortOrderVariant : ICostAndTime
	{
		public int[] StorageIdsForMaterials { get; set; }
		public DeliveryParamsUnit[] DeliveryVariants { get; set; }
		public decimal[] MaterialCosts { get; set; }
		public decimal[] MaterialPricesPerUnit { get; set; }
		public decimal Cost { get; set; }
		public decimal DeliveryTime { get; set; }

		public ShortOrderVariant(int[] storageIdsForMaterials, DeliveryParamsUnit[] deliveryParams, decimal[] materialCosts, decimal[] materialPricesPerUnit, decimal cost, decimal deliveryTime)
		{
			StorageIdsForMaterials = storageIdsForMaterials!;
			DeliveryVariants = deliveryParams;
			MaterialCosts = materialCosts;
			MaterialPricesPerUnit = materialPricesPerUnit;
			Cost = cost;
			DeliveryTime = deliveryTime;
		}
	}

	public interface ICostAndTime
	{
		public decimal Cost { get; set; }
		public decimal DeliveryTime { get; set; }
	}

	public class DeliveryParamsUnit : ICostAndTime
	{
		public int TransportFleetId { get; set; }
		public TransportOnFleet? TransportOnFleet { get; set; }
		public int StorageId { get; set; }
		public Storage? Storage { get; set; }
		public decimal Cost { get; set; }
		public decimal DeliveryTime { get; set; }
		public decimal Distance { get; set; }
	}

	public class TransportOnFleetWithRegions
	{
		public TransportOnFleet? TransportOnFleet { get; init; }
		public int?[] RegionIds { get; set; }
	}


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
		public int CompanyId { get; init; }
		public string CompanyName { get; init; } = null!;
		public int CompanyTypeId { get; init; }
	}

	public class StorageMaterial
	{
		public int StorageId { get; init; }
		public string StorageName { get; init; } = null!;
		public int ConstructionUnitId { get; init; }
		public int ConstructionUnitTypeId { get; init; }
		public int ManufacturerId { get; init; }
		public decimal PricePerUnit { get; init; }
	}

	public class ConstructionUnitSupplemented : ConstructionUnit
	{
		public string MeasureUnitName { get; init; } = null!;
		public string TypeName { get; init; } = null!;
		public double Amount { get; init; }
	}
}
