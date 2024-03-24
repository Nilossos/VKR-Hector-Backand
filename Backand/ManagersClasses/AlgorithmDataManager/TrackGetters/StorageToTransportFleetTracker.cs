using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
	public class StorageToTransportFleetTracker : TracksGetter<StorageToTransportFleetDistance>
	{
		public StorageToTransportFleetTracker(ApplicationContext dbContext, DistanceService distanceService) : base(dbContext, distanceService) { }

		protected override DbSet<StorageToTransportFleetDistance> TrackDbTable => dbContext.StorageToTransportFleetDistance;

		protected override StorageToTransportFleetDistance ConstructNewTrack(MissingDistance missing, decimal distance) => 
			new() 
			{ 
				Distance = distance, 
				StorageId=missing.Index1, 
				TransportFleetId=missing.Index2
			};

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetStartpoints() =>
			await dbContext.Storage.Select(s => new UnitIdWithCoordinates { Id = s.StorageId, Coordinates = s.Coordinates }).ToListAsync();

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetEndpoints() =>
			await dbContext.TransportFleet.Select(f => new UnitIdWithCoordinates { Id = f.TransportFleetId, Coordinates = f.Coordinates }).ToListAsync();

		protected override bool IsRequiredDistance(StorageToTransportFleetDistance distance, UnitIdWithCoordinates storage, UnitIdWithCoordinates transportFleet) =>
			distance.TransportFleetId == transportFleet.Id && distance.StorageId == storage.Id;
	}
}
