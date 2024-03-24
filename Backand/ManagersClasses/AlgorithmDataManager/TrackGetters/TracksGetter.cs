using Backand.AlgorithmEntities;
using Backand.Services;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager.TrackGetters
{
	class MissingFinderProps<TTrack>
	{
		public IEnumerable<UnitIdWithCoordinates> startpoints;
		public IEnumerable<UnitIdWithCoordinates> endpoints;
		public List<MissingDistance> missingDistances;
		public List<TTrack> distances;
	}

	public abstract class TracksGetter<TTrack> where TTrack : class
	{
		protected readonly ApplicationContext dbContext;
		protected readonly DistanceService distanceService;
		protected abstract DbSet<TTrack> TrackDbTable { get; }
		public TracksGetter(ApplicationContext dbContext, DistanceService distanceService)
		{
			this.dbContext = dbContext;
			this.distanceService = distanceService;
		}

		protected abstract Task<IEnumerable<UnitIdWithCoordinates>> GetEndpoints();
		protected abstract Task<IEnumerable<UnitIdWithCoordinates>> GetStartpoints();
		protected abstract bool IsRequiredDistance(TTrack distance, UnitIdWithCoordinates startpoint, UnitIdWithCoordinates endpoint);
		protected abstract TTrack ConstructNewTrack(MissingDistance missing, decimal distance);

		private void FindMissingDistances(MissingFinderProps<TTrack> props)
		{
			var endpoints = props.endpoints;
			var startpoints = props.startpoints;
			foreach (var startpoint in startpoints)
			{
				foreach (var endpoint in endpoints)
				{
					int index = props.distances.FindIndex(d => IsRequiredDistance(d, startpoint, endpoint));
					if (index == -1)
					{
						props.missingDistances.Add(new MissingDistance
						{
							Index1 = startpoint.Id,
							Index2 = endpoint.Id,
							Coordinates1 = startpoint.Coordinates,
							Coordinates2 = endpoint.Coordinates
						});
					}
				}
			}
		}

		private async Task<decimal> GetMissingDistance(MissingDistance missingDistance)
		{
			double[] coord1 = new double[] { missingDistance.Coordinates1.X, missingDistance.Coordinates1.Y };
			double[] coord2 = new double[] { missingDistance.Coordinates2.X, missingDistance.Coordinates2.Y };
			double[][] routes = new double[][] { coord1, coord2 };

			return Convert.ToDecimal(await distanceService.GetDistance(routes));
		}

		private async Task FillDistancesToDbAdd(List<MissingDistance> missingDistances, List<TTrack> distancesToDbAdd)
		{
			for (int i = 0; i < missingDistances.Count; i++)
			{
				decimal distance = await GetMissingDistance(missingDistances[i]);

				distancesToDbAdd.Add(ConstructNewTrack(missingDistances[i], distance));
			}
		}

		public async Task<List<TTrack>> GetTrackToEndpoints()
		{
			IEnumerable<UnitIdWithCoordinates> startpoints = await GetStartpoints();
			IEnumerable<UnitIdWithCoordinates> endpoints = await GetEndpoints();

			List<TTrack> distances = await TrackDbTable.ToListAsync();
			List<MissingDistance> missingDistances = new();
			List<TTrack> distancesToDbAdd = new();

			MissingFinderProps<TTrack> props = new()
			{
				distances = distances,
				missingDistances = missingDistances,
				startpoints = startpoints,
				endpoints = endpoints
			};
			FindMissingDistances(props);
			await FillDistancesToDbAdd(missingDistances, distancesToDbAdd);
			distances.AddRange(distancesToDbAdd);
			await TrackDbTable.AddRangeAsync(distancesToDbAdd);
			await dbContext.SaveChangesAsync();
			return distances;
		}
	}
}
