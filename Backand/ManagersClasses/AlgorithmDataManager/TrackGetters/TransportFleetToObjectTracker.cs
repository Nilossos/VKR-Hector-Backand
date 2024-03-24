using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
	public class TransportFleetToObjectTracker : TracksGetter<TransportFleetToObjectsDistance>
	{
		public TransportFleetToObjectTracker(ApplicationContext dbContext, DistanceService distanceService) : base(dbContext, distanceService) { }

		protected override DbSet<TransportFleetToObjectsDistance> TrackDbTable => dbContext.TransportFleetToObjectsDistance;

		protected override TransportFleetToObjectsDistance ConstructNewTrack(MissingDistance missing, decimal distance) =>
			new()
			{
				TransportFleetId = missing.Index1,
				ObjectsId = missing.Index2,
				Distance = distance
			};

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetStartpoints() =>
			await dbContext.TransportFleet.Select(s => new UnitIdWithCoordinates { Id = s.TransportFleetId, Coordinates = s.Coordinates }).ToListAsync();

		protected override async Task<IEnumerable<UnitIdWithCoordinates>> GetEndpoints() =>
			await dbContext.Objects.Select(tf => new UnitIdWithCoordinates { Id = tf.ObjectsId, Coordinates = tf.Coordinates }).ToListAsync();

		protected override bool IsRequiredDistance(TransportFleetToObjectsDistance distance, UnitIdWithCoordinates transportFleet, UnitIdWithCoordinates objects) =>
			distance.ObjectsId == objects.Id && distance.TransportFleetId == transportFleet.Id;
	}
}
