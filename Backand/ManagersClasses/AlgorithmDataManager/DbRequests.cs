using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace Backand.ManagersClasses.AlgorithmDataManager
{
	public class DbRequests
	{
		protected static async Task<List<StorageMaterial>> GetStoragesMaterialsAsync(ApplicationContext dbContext) =>
			await (from storConstrUnit in dbContext.Storage_ConstructionUnit
				   join storage in dbContext.Storage on storConstrUnit.StorageId equals storage.StorageId
				   join constrUnit in dbContext.ConstructionUnit on storConstrUnit.ConstructionUnitId equals constrUnit.ConstructionUnitId
				   join manufacturer in dbContext.Manufacturer on storage.ManufacturerId equals manufacturer.ManufacturerId
				   select new StorageMaterial
				   {
					   StorageId = storage.StorageId,
					   StorageName = storage.Name,
					   ConstructionUnitId = constrUnit.ConstructionUnitId,
					   ConstructionUnitTypeId = constrUnit.ConstructionUnitTypeId,
					   ManufacturerId = manufacturer.ManufacturerId,
					   PricePerUnit = storConstrUnit.PricePerUnit
				   }).ToListAsync();

		protected static async Task<List<TransportOnFleetWithRegions>> GetTransportsOnFleetsAsync(ApplicationContext dbContext)
		{
			var companyNamesQuery = (from logCompany in dbContext.LogisticCompany select new { CompanyId = logCompany.LogisticCompanyId, CompanyName = logCompany.Name })
				.Union(from manufact in dbContext.Manufacturer select new { CompanyId = manufact.ManufacturerId, CompanyName = manufact.Name });

			var query = from deliveryRegion in dbContext.DeliveryRegion
						join trOnFleet in dbContext.TransportFleet_Transport on deliveryRegion.TransportFleet_TransportId equals trOnFleet.TransportFleet_TransportId
						join transport in dbContext.Transport on trOnFleet.TransportId equals transport.TransportId
						join trMode in dbContext.TransportMode on transport.TransportModeId equals trMode.TransportModeId
						join trType in dbContext.TransportType on trMode.TransportTypeId equals trType.TransportTypeId
						join trFleet in dbContext.TransportFleet on trOnFleet.TransportFleetId equals trFleet.TransportFleetId
						join company in dbContext.Company on trFleet.CompanyId equals company.CompanyId
						join companyName in companyNamesQuery on company.CompanyId equals companyName.CompanyId
						join coefType in dbContext.CoefficientType on trOnFleet.CoefficientTypeId equals coefType.CoefficientTypeId
						select new
						{
							deliveryRegion.RegionId,
							TransportOnFleet = new TransportOnFleet
							{
								TransportId = transport.TransportId,
								TransportName = transport.Name,
								TransportFleet = trFleet,
								CoefficientTypeId = trOnFleet.CoefficientTypeId,
								CoefficientTypeName = coefType.Name,
								CoefficientValue = trOnFleet.CoefficientValue,
								TransportTypeId = trType.TransportTypeId,
								TransportTypeName = trType.Name,
								TransportModeName = trMode.Name,
								AverageSpeed = trOnFleet.AverageSpeed,
								CompanyId = company.CompanyId,
								CompanyName = companyName.CompanyName,
								CompanyTypeId = company.CompanyTypeId
							}
						};

			var data = (await query.ToListAsync())
				.GroupBy(t => t.TransportOnFleet)
				.Select(g => new TransportOnFleetWithRegions
				{
					TransportOnFleet = g.Key,
					RegionIds = g.Select(r => (int)r.RegionId!).ToArray()
				})
				.ToList();

			return data;
		}

		protected static Dictionary<int, List<ConstructionUnitSupplemented>> GetMaterialsSetsWithConstructionTypes(List<MaterialSet> materialSets, ApplicationContext dbContext, int constructionTypeId) =>
			(from mSet in materialSets
			 where mSet.ConstructionTypeId == constructionTypeId
			 join mSet_cUnit in dbContext.MaterialSet_ConstructionUnit on mSet.MaterialSetId equals mSet_cUnit.MaterialSetId
			 join cUnit in dbContext.ConstructionUnit on mSet_cUnit.ConstructionUnitId equals cUnit.ConstructionUnitId
			 join cUnitType in dbContext.ConstructionUnitType on cUnit.ConstructionUnitTypeId equals cUnitType.ConstructionUnitTypeId
			 join measureUnit in dbContext.MeasureUnit on cUnit.MeasureUnitId equals measureUnit.MeasureUnitId
			 select new
			 {
				 mSet.MaterialSetId,
				 ConstructionUnitWithAmount = new ConstructionUnitSupplemented
				 {
					 ConstructionUnitId = cUnit.ConstructionUnitId,
					 ConstructionUnitTypeId = cUnit.ConstructionUnitTypeId,
					 MeasureUnitId = cUnit.MeasureUnitId,
					 Name = cUnit.Name,
					 TypeName = cUnitType.Name,
					 MeasureUnitName = measureUnit.Name,
					 Amount = mSet_cUnit.Amount
				 }
			 })
			.GroupBy(m => m.MaterialSetId)
			.ToDictionary(
				group => group.Key,
				group => group.Select(g => g.ConstructionUnitWithAmount).ToList()
			);

		internal static Dictionary<int, Manufacturer> GetManufacturersByStorageIds(int[] storageIds, ApplicationContext dbContext, AlgorithmData data) =>
			data.storages
				.Where(s => storageIds.Contains(s.StorageId))
				.Join(dbContext.Manufacturer,
				s => s.ManufacturerId,
				m => m.ManufacturerId,
				(s, m) => new { s.StorageId, Manufacturer = m })
				.ToDictionary(o => o.StorageId, o => o.Manufacturer);
	}
}
