using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
	public class StorageToObjectTracker : TracksGetter<StorageToObjectDistance>
	{
		public StorageToObjectTracker(ApplicationContext dbContext, DistanceService distanceService) : base(dbContext, distanceService) { }

		protected override DbSet<StorageToObjectDistance> TrackDbTable => dbContext.StorageToObjectDistance;

		protected override StorageToObjectDistance ConstructNewTrack(MissingDistance missing, decimal distance) =>
			new()
			{
				StorageId = missing.Index1,
				ObjectId = missing.Index2,
				Distance = distance
			};

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetStartpoints() =>
			await dbContext.Storage.Select(s => new UnitIdWithCoordinates { Id = s.StorageId, Coordinates = s.Coordinates }).ToListAsync();

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetEndpoints() => 
			await dbContext.Object.Select(s => new UnitIdWithCoordinates { Id = s.ObjectId, Coordinates = s.Coordinates }).ToListAsync();

		protected override bool IsRequiredDistance(StorageToObjectDistance distance, UnitIdWithCoordinates storage, UnitIdWithCoordinates @object) =>
			distance.StorageId == storage.Id && distance.ObjectId == @object.Id;
	}
}
