using Backand.DbEntities;

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