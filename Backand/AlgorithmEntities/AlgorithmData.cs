using Backand.DbEntities;
using Backand.DbEntities.ConstructionSpace;
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
		List<StorageToObjectDistance> storageToObjectDistances,
		List<StorageToTransportFleetDistance> storageToTransportFleetDistances,
		List<TransportFleetToObjectDistance> transportFleetToObjectDistances
	);
}