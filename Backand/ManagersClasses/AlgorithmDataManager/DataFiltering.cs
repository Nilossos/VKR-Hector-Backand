using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.FrontendEntities.Requests;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class DataFiltering
	{
		internal static List<TransportOnFleetWithRegions> FilterFleetsByLogisticCompanies(List<TransportOnFleetWithRegions> transportsOnFleets, ConstructionOption constructionOption)
		{
			if (constructionOption.Filter.CertainManufacturers.Ids.Count > 0)
				return transportsOnFleets
					.Where(t => constructionOption.Filter.CertainLogists.Ids.Contains(t.TransportOnFleet!.CompanyId))
					.ToList();
			else
				return transportsOnFleets;
		}

		internal static List<StorageMaterial> FilterMaterialsByManufacturers(List<StorageMaterial> storagesMaterials, ConstructionOption constructionOption)
		{
			if (constructionOption.Filter.CertainLogists.Ids.Count > 0)
				return storagesMaterials
					.Where(m => constructionOption.Filter.CertainManufacturers.Ids.Contains(m.ManufacturerId))
					.ToList();
			else
				return storagesMaterials;
		}

		internal static List<TransportOnFleetWithRegions> FilterTransportFromTransportFleetByType(List<TransportOnFleetWithRegions> transportsOnFleets, int transportFleetId, TransportTypeValue transportTypeValue) =>
			transportsOnFleets
			.Where(
				t => t.TransportOnFleet.TransportFleet.TransportFleetId == transportFleetId
				&& t.TransportOnFleet.TransportTypeId == (int)transportTypeValue
			).ToList();
	}
}
