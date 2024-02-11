using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
    public class TransportFleetToObjectTracker : TracksGetter<TransportFleetToObjectsDistance>
    {
        public TransportFleetToObjectTracker(ApplicationContext dbContext, DistanceService distanceService) : 
            base(dbContext, distanceService) { }

        protected override DbSet<TransportFleetToObjectsDistance> TrackDbTable => dbContext.TransportFleetToObjectsDistance;

        protected override TransportFleetToObjectsDistance ConstructNewTrack(MissingDistance missing, decimal distance)
        {
            return new()
            {
                TransportFleetId = missing.Index1,
                ObjectsId = missing.Index2,
                Distance = distance
            };
        }

        protected override IEnumerable<UnitIdWithCoordinates> GetEndpoints()
        {
            return dbContext.Objects.Select(tf => 
                    new UnitIdWithCoordinates { Id = tf.ObjectsId, Coordinates = tf.Coordinates });
        }

        protected override IEnumerable<UnitIdWithCoordinates> GetStartpoints()
        {
            return dbContext.TransportFleet.Select(s => new UnitIdWithCoordinates { Id = s.TransportFleetId, Coordinates = s.Coordinates });
        }

        protected override bool IsRequiredDistance(TransportFleetToObjectsDistance distance, UnitIdWithCoordinates startpoint, UnitIdWithCoordinates endpoint)
        {
            return distance.ObjectsId == startpoint.Id && distance.TransportFleetId == endpoint.Id;
        }
    }
}
