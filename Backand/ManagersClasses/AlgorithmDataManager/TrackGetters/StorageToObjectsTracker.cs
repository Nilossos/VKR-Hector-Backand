using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
	public class StorageToObjectsTracker : TracksGetter<StorageToObjectsDistance>
	{
		public StorageToObjectsTracker(ApplicationContext dbContext, DistanceService distanceService) : base(dbContext, distanceService) { }

		protected override DbSet<StorageToObjectsDistance> TrackDbTable => dbContext.StorageToObjectsDistance;

		protected override StorageToObjectsDistance ConstructNewTrack(MissingDistance missing, decimal distance) =>
			new()
			{
				StorageId = missing.Index1,
				ObjectsId = missing.Index2,
				Distance = distance
			};

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetStartpoints() =>
			await dbContext.Storage.Select(s => new UnitIdWithCoordinates { Id = s.StorageId, Coordinates = s.Coordinates }).ToListAsync();

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetEndpoints() => 
			await dbContext.Objects.Select(s => new UnitIdWithCoordinates { Id = s.ObjectsId, Coordinates = s.Coordinates }).ToListAsync();

		protected override bool IsRequiredDistance(StorageToObjectsDistance distance, UnitIdWithCoordinates storage, UnitIdWithCoordinates objects) =>
			distance.StorageId == storage.Id && distance.ObjectsId == objects.Id;
	}
}
