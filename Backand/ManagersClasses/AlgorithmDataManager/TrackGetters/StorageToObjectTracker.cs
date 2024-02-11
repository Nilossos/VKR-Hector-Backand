using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
    public class StorageToObjectTracker : TracksGetter<StorageToObjectsDistance>
    {
        public StorageToObjectTracker(ApplicationContext dbContext, DistanceService distanceService) : base(dbContext, distanceService) { }
        protected override DbSet<StorageToObjectsDistance> TrackDbTable => dbContext.StorageToObjectDistance;
        protected override StorageToObjectsDistance ConstructNewTrack(MissingDistance missing, decimal distance) =>
            new()
            {
                StorageId = missing.Index1,
                ObjectsId = missing.Index2,
                Distance = distance
            };
        protected override IEnumerable<UnitIdWithCoordinates> GetStartpoints()
        {
            return dbContext.Storage.Select(s => new UnitIdWithCoordinates { Id = s.StorageId, Coordinates = s.Coordinates });
        }
        protected override IEnumerable<UnitIdWithCoordinates> GetEndpoints() => 
            dbContext.Objects.Select(s => new UnitIdWithCoordinates { Id = s.ObjectsId, Coordinates = s.Coordinates });

        protected override bool IsRequiredDistance(StorageToObjectsDistance distance, UnitIdWithCoordinates storage, UnitIdWithCoordinates endpoint) =>
            distance.StorageId == storage.Id && distance.ObjectsId == endpoint.Id;
    }
}
