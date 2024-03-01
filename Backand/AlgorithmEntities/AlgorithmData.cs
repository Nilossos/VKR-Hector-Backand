using Backand.DbEntities;
<<<<<<< HEAD
using Backand.DbEntities.ConstructionSpace;
using NpgsqlTypes;
=======
>>>>>>> 98622d32a5e8c5f4fc67991190d9931116e1e6e3

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
		List<StorageToTransportFleetDistance> storageToTransportFleetDistances,
		List<TransportFleetToObjectsDistance> transportFleetToObjectsDistances
	);
}