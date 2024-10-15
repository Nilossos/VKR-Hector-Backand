using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
    public class TransportFleetToObjectTracker : TracksGetter<TransportFleetToObjectDistance>
    {
		public TransportFleetToObjectTracker(ApplicationContext dbContext, DistanceService distanceService) : base(dbContext, distanceService) { }

        protected override DbSet<TransportFleetToObjectDistance> TrackDbTable => dbContext.TransportFleetToObjectDistance;

        protected override TransportFleetToObjectDistance ConstructNewTrack(MissingDistance missing, decimal distance) =>
            new()
            {

                TransportFleetId = missing.Index1,
                ObjectId = missing.Index2,
                Distance = distance
            };

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetStartpoints() =>
			await dbContext.TransportFleet.Select(s => new UnitIdWithCoordinates(s.TransportFleetId, s.Coordinates)).ToListAsync();

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetEndpoints() =>
			await dbContext.Object.Select(tf => new UnitIdWithCoordinates { Id = tf.ObjectId, Coordinates = tf.Coordinates }).ToListAsync();

		protected override bool IsRequiredDistance(TransportFleetToObjectDistance distance, UnitIdWithCoordinates transportFleet, UnitIdWithCoordinates @object) =>
			distance.ObjectId == @object.Id && distance.TransportFleetId == transportFleet.Id;
	}
}
