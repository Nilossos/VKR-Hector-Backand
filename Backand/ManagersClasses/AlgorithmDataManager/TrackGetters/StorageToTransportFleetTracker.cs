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
        protected override StorageToTransportFleetDistance ConstructNewTrack(MissingDistance missing, decimal distance)
        {
            return new StorageToTransportFleetDistance { Distance = distance, StorageId=missing.Index1, TransportFleetId=missing.Index2};
        }
        protected override IEnumerable<UnitIdWithCoordinates> GetEndpoints()
        {
            return dbContext.TransportFleet.Select(f => new UnitIdWithCoordinates { 
                Id = f.TransportFleetId,
                Coordinates = f.Coordinates });
        }
        protected override IEnumerable<UnitIdWithCoordinates> GetStartpoints()
        {
            return dbContext.Storage.Select(s => new UnitIdWithCoordinates { Id = s.StorageId, Coordinates = s.Coordinates });
        }
        protected override bool IsRequiredDistance(StorageToTransportFleetDistance distance, UnitIdWithCoordinates storage, UnitIdWithCoordinates endpoint)
        {
            return distance.TransportFleetId == storage.Id && distance.StorageId == endpoint.Id;
        }
    }
}
