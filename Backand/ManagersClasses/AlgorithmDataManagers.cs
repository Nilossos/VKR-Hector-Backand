using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;
using Dadata;
using Dadata.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Backand.ManagersClasses
{
	public class AlgorithmDataManagers
	{
		public static async Task GetFinalCostTime(HttpContext context, ApplicationContext dbContext)
		{
			AlgorithmRequest algorithmRequest = await context.Request.ReadFromJsonAsync<AlgorithmRequest>();
			List<ConstructionOption> constructionOptions = algorithmRequest.ConstructionOptions;

			//var a = algorithmRequest.ConstructionOptions;
			//apiResult.geo_lat and apiResult.geo_lon

			List<Storage> storages = dbContext.Storage.ToList();
			List<TransportFleet> transportFleets = dbContext.TransportFleet.ToList();
			var transportsOnFleets =
				from trOnFleet in dbContext.TransportFleet_Transport
				join transport in dbContext.Transport on trOnFleet.TransportId equals transport.TransportId
				join trMode in dbContext.TransportMode on transport.TransportModeId equals trMode.TransportModeId
				join trType in dbContext.TransportType on trMode.TransportTypeId equals trType.TransportTypeId
				select new
				{
					trOnFleet.TransportFleetId,
					transport.TransportId,
					trOnFleet.CoefficientTypeId,
					trOnFleet.CoefficientValue,
					trType.TransportTypeId,
					trOnFleet.AverageSpeed,
					TransportTypeName = trType.Name,
					TransportName = transport.Name
				};

			double[,] deliveryCosts = new double[transportFleets.Count, storages.Count];
			double[,] storageTransportFleetDistances = new double[transportFleets.Count, storages.Count];
			// TODO: запросом к JS API заполнить расстояния от складов до парков транспорта

			foreach (var constructionOption in constructionOptions)
			{
				Dictionary<int, int> storageToObjectDistances = new();
				// TODO: через запрос к JS API заполнить словарь расстояний от склада до объекта

				for (int transportFleetIndex = 0; transportFleetIndex < transportFleets.Count; transportFleetIndex++)
				{
					var transportsOnFleet = transportsOnFleets
						.Where(
							t => t.TransportFleetId == transportFleets[transportFleetIndex].TransportFleetId
							&& t.TransportTypeId == 2 //наземный транспорт
						);
					var bestTransport = transportsOnFleet.MinBy(t => t.AverageSpeed);

					for (int storageIndex = 0; storageIndex < storages.Count; storageIndex++)
						deliveryCosts[transportFleetIndex, storageIndex] = bestTransport.CoefficientValue * (storageTransportFleetDistances[transportFleetIndex, storageIndex] + storageToObjectDistances[storageIndex]);
				}
			}



			



			//List<CoefficientType> list;
			//using (ApplicationContext db = new ApplicationContext())
			//{
			//	list = db.CoefficientType.ToList();
			//	await context.Response.WriteAsJsonAsync(list);
			//}

			//context.Response.StatusCode = StatusCodes.Status404NotFound;
		}
	}
}
