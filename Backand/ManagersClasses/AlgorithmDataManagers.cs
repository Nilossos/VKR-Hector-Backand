using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backand.ManagersClasses
{
	public class AlgorithmDataManagers
	{
		public static async Task<double[,]> GetStorageToObjectDistances(string[] storageAddresses, string[] objectAddresses)
		{
			return new double[1, 1];
		}

		public static async Task<double[,]> GetStorageTransportFleetDistances(string[] storageAddresses, string[] transportFleeetDistances)
		{
			var data = new
			{
				storageAddresses,
				transportFleeetDistances
			};

			string jsonData = JsonConvert.SerializeObject(data);
			using HttpClient client = new HttpClient();
			HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

			// UPDATE: прописать apiUrl
			string apiUrl = "";
			HttpResponseMessage response = await client.PostAsync(apiUrl, content);

			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				double[,] distanceMatrix = JsonConvert.DeserializeObject<double[,]>(responseContent);

				// DEBUG OUTPUT
				for (int i = 0; i < distanceMatrix.GetLength(0); i++)
				{
					for (int j = 0; j < distanceMatrix.GetLength(1); j++)
					{
						Console.Write(distanceMatrix[i, j] + "\t");
					}
					Console.WriteLine();
				}
				return distanceMatrix;
			}
			else
			{
				Console.WriteLine("Ошибка при выполнении запроса. Код ответа: " + response.StatusCode);
				return new double[1,1];
			}
		}

		public static async Task CalculateOrderCostTime(HttpContext context, ApplicationContext dbContext)
		{
			AlgorithmRequest? algorithmRequest = await context.Request.ReadFromJsonAsync<AlgorithmRequest>();
			List<ConstructionOption> constructionOptions = algorithmRequest.ConstructionOptions;

			//все склады
			List<Storage> storages = await dbContext.Storage.ToListAsync();

			//все парки транспорта
			List<TransportFleet> transportFleets = await dbContext.TransportFleet.ToListAsync();

			//весь транспорт из парков транспорта
			List<TransportOnFleet> transportsOnFleetsAll = await
				(from trOnFleet in dbContext.TransportFleet_Transport
				 join transport in dbContext.Transport on trOnFleet.TransportId equals transport.TransportId
				 join trMode in dbContext.TransportMode on transport.TransportModeId equals trMode.TransportModeId
				 join trType in dbContext.TransportType on trMode.TransportTypeId equals trType.TransportTypeId
				 join trFleet in dbContext.TransportFleet on trOnFleet.TransportFleetId equals trFleet.TransportFleetId
				 join company in dbContext.Company on trFleet.CompanyId equals company.CompanyId
				 select new TransportOnFleet
				 (
					 trOnFleet.TransportFleetId,
					 transport.TransportId,
					 trOnFleet.CoefficientTypeId,
					 trOnFleet.CoefficientValue,
					 trType.TransportTypeId,
					 trOnFleet.AverageSpeed,
					 trType.Name,
					 transport.Name,
					 company.CompanyId,
					 company.CompanyTypeId
				 )).ToListAsync();

			//все наборы материалов
			List<MaterialSet> materialSets = await dbContext.MaterialSet.ToListAsync();

			//все материалы со всех складов
			List<StorageMaterial> storagesMaterialsAll = await
				(from storConstrUnit in dbContext.Storage_ConstructionUnit
				 join storage in dbContext.Storage on storConstrUnit.StorageId equals storage.StorageId
				 join constrUnit in dbContext.ConstructionUnit on storConstrUnit.ConstructionUnitId equals constrUnit.ConstructionUnitId
				 join manufacturer in dbContext.Manufacturer on storage.ManufacturerId equals manufacturer.ManufacturerId
				 select new StorageMaterial {
					 StorageId = storage.StorageId,
					 StorageName = storage.Name,
					 ConstructionUnitId = constrUnit.ConstructionUnitId,
					 ConstructionUnitTypeId = constrUnit.ConstructionUnitTypeId,
					 ManufacturerId = manufacturer.ManufacturerId,
					 PricePerUnit = storConstrUnit.PricePerUnit
				 }).ToListAsync();

			List<DataElement> deliveryCosts = new();
			double[,] storageTransportFleetDistances = new double[transportFleets.Count, storages.Count];
			// TODO: запросом к JS API заполнить расстояния от складов до парков транспорта

			//цикл, в котором создаются варианты заказа для каждого сооружения
			foreach (var constructionOption in constructionOptions)
			{
				List<TransportOnFleet> transportsOnFleets;
				List<StorageMaterial> storagesMaterials;

				if (constructionOption.Filter.CertainManufacturers.Ids.Count > 0) //фильтр по логистическим компаниям
					transportsOnFleets = transportsOnFleetsAll
						.Where(t => constructionOption.Filter.CertainLogists.Ids.Contains(t.CompanyId))
						.ToList();
				else
					transportsOnFleets = transportsOnFleetsAll;

				if (constructionOption.Filter.CertainLogists.Ids.Count > 0) //фильтр по производителям
					storagesMaterials = storagesMaterialsAll
						.Where(m => constructionOption.Filter.CertainManufacturers.Ids.Contains(m.ManufacturerId))
						.ToList();
				else
					storagesMaterials = storagesMaterialsAll;


				Dictionary<int, int> storageToObjectDistances = new();
				// TODO: через запрос к JS API заполнить словарь расстояний от склада до объекта
				// TODO: реализовать доступ к сущностям в бд, которые содержат расстояния

				//цикл, в котором рассчитываются стоимости доставок от склада до объекта
				for (int transportFleetIndex = 0; transportFleetIndex < transportFleets.Count; transportFleetIndex++)
				{
					var transportsOnFleet = transportsOnFleets
						.Where(
							t => t.TransportFleetId == transportFleets[transportFleetIndex].TransportFleetId
							&& t.TransportTypeId == 2 //наземный транспорт
						).ToList();
					var bestTransport = transportsOnFleet.Sort((t1, t2) => t1.AverageSpeed,);

					for (int storageIndex = 0; storageIndex < storages.Count; storageIndex++)
						deliveryCosts.Add(new DataElement
						{
							Row = transportFleetIndex,
							Column = storageIndex,
							Value = bestTransport.CoefficientValue * (storageTransportFleetDistances[transportFleetIndex, storageIndex] + storageToObjectDistances[storageIndex])
						});
				}
				deliveryCosts.Sort((e1, e2) => e1.Value.CompareTo(e2.Value));

				//формируем матрицу Storage_ConstructionUnits
				//достаём все элементы, которые могут содержаться в сооружении (то есть для начала берём комплект для сооружения)
				int constructionTypeId = dbContext.Construction.FirstOrDefault(c => c.ConstructionId == constructionOption.ConstructionId).ConstructionTypeId;

				//берём наборы для конкретного сооружения из заказа и добавляем к ним тип материала
				var constructionMaterialSets = materialSets
					.Where(mSet => mSet.ConstructionTypeId == constructionTypeId)
					.Join(
						dbContext.MaterialSet_ConstructionUnit,
						mSet => mSet.MaterialSetId,
						mSet_cUnit => mSet_cUnit.MaterialSetId,
						(mSet, mSet_cUnit) => new { 
							mSet.MaterialSetId, 
							mSet_cUnit.ConstructionUnitId,
							mSet_cUnit.Amount
						}
					).Join(
						dbContext.ConstructionUnit,
						sets => sets.ConstructionUnitId,
						cUnit => cUnit.ConstructionUnitId,
						(sets, cUnit) => new { 
							sets.MaterialSetId,
							cUnit.ConstructionUnitId,
							cUnit.ConstructionUnitTypeId,
							sets.Amount
						}
					)
					.GroupBy(sets => sets.MaterialSetId)
					.ToDictionary(
						group => group.Key,
						group => group.Select(item => new { item.ConstructionUnitId, item.ConstructionUnitTypeId, item.Amount }).ToList()
					);

				BuildType allowedBuildType = constructionOption.Filter.BuildType;


				foreach (var constructionMaterialSet in constructionMaterialSets)
				{
					bool notAllowedBuildTypeFound = false;
					var constructionUnits = constructionMaterialSet.Value;

                    //foreach (var constructionUnit in constructionUnits)
                    //	if (allowedBuildType != BuildType.NoMatter && (BuildType)constructionUnit.ConstructionUnitTypeId == allowedBuildType)
                    //	{
                    //		notAllowedBuildTypeFound = true;
                    //		break;
                    //	}

                    //пропускаем тип постройки, который отключен фильтро
                    if (allowedBuildType != BuildType.NoMatter && (BuildType)constructionUnits[0].ConstructionUnitTypeId == allowedBuildType)
						continue;

					int[] uniqueConstructionUnitIds = constructionUnits.Select(cUnit => cUnit.ConstructionUnitId).ToArray();
					var constructionUnitsFromStorage = storagesMaterials.Where(sm => uniqueConstructionUnitIds.Contains(sm.ConstructionUnitId)).ToList();

					int[] uniqueStorageIds = constructionUnitsFromStorage.Select(cUnit => cUnit.StorageId).Distinct().ToArray();
					decimal?[,] storageMaterialMatrix = new decimal?[uniqueStorageIds.Length, uniqueConstructionUnitIds.Length];
					decimal? costValue;

					for (int storageId = 0; storageId < uniqueStorageIds.Length; storageId++)
					{
						for (int materialId = 0; materialId < uniqueConstructionUnitIds.Length; materialId++)
						{
							var constructionUnitFromStorage = constructionUnitsFromStorage
								.FirstOrDefault(cUnit =>
								cUnit!.ConstructionUnitId == uniqueConstructionUnitIds[materialId]
								&& cUnit.StorageId == uniqueStorageIds[storageId], null
							);

							if (constructionUnitFromStorage != null)
								costValue = constructionUnitFromStorage.PricePerUnit * (decimal)constructionUnits[materialId].Amount;
							else
								costValue = null;
							storageMaterialMatrix[storageId, materialId] = costValue;
						}
					}
				}


				//Бахнуть перебор
				for ()
				{

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
