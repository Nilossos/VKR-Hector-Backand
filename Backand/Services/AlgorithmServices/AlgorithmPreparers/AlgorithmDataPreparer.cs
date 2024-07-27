﻿using Backand.AlgorithmEntities;
using Backand.DbEntities;
using Backand.DbEntities.ConstructionSpace;
using Backand.Services.AlgorithmServices.AlgorithmPreparers.Values;
using Microsoft.EntityFrameworkCore;

namespace Backand.Services.AlgorithmServices.AlgorithmPreparers;

public class AlgorithmDataPreparer
{
	private readonly AlgorithmData _data;
	private readonly ApplicationContext _applicationContext;

	public AlgorithmDataPreparer(AlgorithmData data, ApplicationContext applicationContext) => 
		(_data, _applicationContext) = (data, applicationContext);

	public async Task<Construction?> GetConstructionById(int constructionId, CancellationToken cancellationToken) => await
		_applicationContext
			.Construction
			.Include(construction => construction.Object)
				.ThenInclude(objects => objects!.Mine)
					.ThenInclude(mine => mine!.Subsidiary)
			.Include(construction => construction.ConstructionType)
			.FirstOrDefaultAsync(construction => construction.ConstructionId == constructionId, cancellationToken);

	public async Task<int[]> GetAllTransportTypeIds(CancellationToken cancellationToken) =>
		await _applicationContext.TransportType.Select(t => t.TransportTypeId).ToArrayAsync(cancellationToken);

	public async Task<StorageToObjectsDistance[]> GetStorageToConstructionDistanceInfoVector(Objects constructionObject,
		IEnumerable<int> filterTransportTypes,
		CancellationToken cancellationToken)
	{
		return await _applicationContext.StorageToObjectsDistance
			.Where(deliveryInfo => deliveryInfo.Distance != null &&
			                       deliveryInfo.Objects == constructionObject)
			.Include(distance => distance.Storage)
				.ThenInclude(storage => storage.Manufacturer)
			.Include(distance => distance.Storage)
				.ThenInclude(storage => storage.Storage_ConstructionUnits)
					.ThenInclude(storage => storage.ConstructionUnit)
			.ToArrayAsync(cancellationToken);
	}

	public List<int> GetUniqueStorageIds(IEnumerable<StorageToObjectsDistance> storageToObjectsDistances)
	{
		return storageToObjectsDistances
			.Select(deliveryInfo => deliveryInfo.Storage.StorageId)
			.ToList();
	}

	public decimal[] GetStorageToConstructionDistanceVector(
		IEnumerable<StorageToObjectsDistance> storageToObjectsDistances)
	{
		return storageToObjectsDistances
			.Select(s => s.Distance!.Value)
			.ToArray();
	}

	public async Task<(TransportFleet_Transport transport, TransportFleetToObjectsDistance deliveryInfo)[]> GetTransportsToObjectsDistance(
		ICollection<int> transportFleetIds, ICollection<int> transportTypeCodes, int objectId, CancellationToken cancellationToken)
	{
		var queryResult = await _applicationContext.TransportFleetToObjectsDistance
			.Where(deliveryInfo => deliveryInfo.Distance != null &&
			                       deliveryInfo.ObjectsId == objectId &&
			                       transportFleetIds.Contains(deliveryInfo.TransportFleetId))
			.Where(deliveryInfo => deliveryInfo
				.TransportFleet
				.TransportFleet_Transports
				.Any(transport =>
					transport.Transport != null &&
					transport.Transport.TransportMode != null &&
					transport.Transport.TransportMode.TransportTypeId != null &&
					(TransportTypeValue)transport.Transport.TransportMode.TransportTypeId.Value !=
					TransportTypeValue.Ground &&
					transportTypeCodes.Contains(transport.Transport.TransportMode.TransportTypeId.Value)))
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
			.ThenInclude(fleet => fleet.TransportFleet_Transports)
			.ThenInclude(transport => transport.Transport)
			.ThenInclude(transport => transport!.TransportMode)
			.ThenInclude(mode => mode!.TransportType)
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
			.ThenInclude(fleet => fleet.TransportFleet_Transports)
			.ThenInclude(transport => transport.CoefficientType)
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
			.ThenInclude(fleet => fleet.Company)
			.ThenInclude(company => (company == null) ? null : company.LogisticCompany)
			.ToListAsync(cancellationToken);
		
		return queryResult.SelectMany(deliveryInfo => (deliveryInfo
				.TransportFleet
				.TransportFleet_Transports
				.Where(transport =>
					transport.Transport is { TransportMode.TransportTypeId: not null } &&
					transport.Transport.TransportMode.TransportTypeId.Value != 
					(int)TransportTypeValue.Ground &&
					transportTypeCodes.Contains(transport.Transport.TransportMode.TransportTypeId.Value))
				.Select(transport => (transport, deliveryInfo))))
			.OrderBy(pair => pair.transport.TransportFleet_TransportId)
			.ToArray();
	}
	
    public async Task<(TransportFleet_Transport, StorageToTransportFleetDistance distance)[]> GetStoragesToTransportFleetsInfos(
		ICollection<int> transportIds,
		ICollection<int> storagesIds,
		ICollection<int> transportTypeCodes, int objectId, CancellationToken cancellationToken)
	{
		var queryResult = await _applicationContext.StorageToTransportFleetDistance
			.Where(deliveryInfo => deliveryInfo.Distance != null &&
			                       storagesIds.Contains(deliveryInfo.StorageId))
			.Where(deliveryInfo => deliveryInfo
				.TransportFleet
				.TransportFleet_Transports
				.Any(transport =>
					transportIds.Contains(transport.TransportFleet_TransportId) &&
					transport.Transport != null &&
					transport.Transport.TransportMode != null &&
					transport.Transport.TransportMode.TransportTypeId != null &&
					(TransportTypeValue)transport.Transport.TransportMode.TransportTypeId.Value !=
					TransportTypeValue.Ground &&
					transportTypeCodes.Contains(transport.Transport.TransportMode.TransportTypeId.Value)))
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
			.ThenInclude(fleet => fleet.TransportFleet_Transports)
			.ThenInclude(transport => transport.Transport)
			.ThenInclude(transport => transport!.TransportMode)
			.ThenInclude(mode => mode!.TransportType)
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
			.ThenInclude(fleet => fleet.TransportFleet_Transports)
			.ThenInclude(transport => transport.CoefficientType)
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
			.ThenInclude(fleet => fleet.Company)
			.ThenInclude(company => (company == null) ? null : company.LogisticCompany)
			.ToListAsync(cancellationToken);
		
		return queryResult.SelectMany(deliveryInfo => (deliveryInfo
				.TransportFleet
				.TransportFleet_Transports
				.Where(transport =>
					transport.Transport is { TransportMode.TransportTypeId: not null } &&
					(TransportTypeValue)transport.Transport.TransportMode.TransportTypeId.Value != TransportTypeValue.Ground &&
					transportTypeCodes.Contains(transport.Transport.TransportMode.TransportTypeId.Value))
				.Select(transport => (transport, deliveryInfo))))
			.OrderBy(pair => Tuple.Create(pair.transport.TransportFleet_TransportId,
				pair.deliveryInfo.StorageId))
			.ToArray();
	}
    
	public decimal[,] GetStorageToTransportDistanceMatrix(
		IEnumerable<(TransportFleet_Transport transport, StorageToTransportFleetDistance deliveryInfo)> distanceInformation,
		IList<int> storageIds,
		IList<int> transportIds)
	{
		var result = new decimal[storageIds.Count, transportIds.Count];
		foreach (var transportFleetStorageDistance in distanceInformation)
		{
			var storageIdIndex =
				storageIds.IndexOf(transportFleetStorageDistance.deliveryInfo.StorageId);
			var transportIdIndex =
				transportIds.IndexOf(transportFleetStorageDistance.transport.TransportFleet_TransportId);
			result[storageIdIndex, transportIdIndex] =
				transportFleetStorageDistance.deliveryInfo.Distance!.Value;
		}
		return result;
	}

	public decimal[] GetDistanceVector(
		IEnumerable<(TransportFleet_Transport transport, TransportFleetToObjectsDistance deliveryInfo)> deliveryInfos)
	{
		return deliveryInfos.Where(pair => pair.deliveryInfo.Distance != null)
			.Select(pair => pair.deliveryInfo.Distance!.Value).ToArray();
	}
	
	
	public async Task<(TransportFleet_Transport, StorageToTransportFleetDistance)[]> GetTransportToStorageDistanceInfo(
		ICollection<int> storageIds, IEnumerable<int> transportTypeIds, CancellationToken cancellationToken)
	{
		var queryResult =  await _applicationContext.StorageToTransportFleetDistance
			.Where(deliveryInfo => deliveryInfo.Distance != null &&
			                       storageIds.Contains(deliveryInfo.StorageId))
			.Where(deliveryInfo => deliveryInfo
				.TransportFleet
						.TransportFleet_Transports
						.Any(transport =>
							transport.Transport != null &&
							transport.Transport.TransportMode != null &&
							transport.Transport.TransportMode.TransportTypeId != null &&
							transport.Transport.TransportMode.TransportTypeId.Value == (int)TransportTypeValue.Ground))
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
				.ThenInclude(fleet => fleet.TransportFleet_Transports)
					.ThenInclude(transport => transport.Transport)
						.ThenInclude(transport => transport!.TransportMode)
							.ThenInclude(mode => mode!.TransportType)
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
				.ThenInclude(fleet => fleet.TransportFleet_Transports)
					.ThenInclude(transport => transport.CoefficientType)
			.Include(deliveryInfo => deliveryInfo.TransportFleet)
				.ThenInclude(fleet => fleet.Company)
					.ThenInclude(company => (company == null) ? null : company.LogisticCompany)
			.ToListAsync(cancellationToken);
		
			return queryResult.SelectMany(deliveryInfo => (deliveryInfo
				.TransportFleet
				.TransportFleet_Transports
				.Where(transport =>
					transport.Transport is { TransportMode.TransportTypeId: not null })
				.Select(transport => (transport, deliveryInfo))))
			.OrderBy(pair => Tuple.Create(pair.transport.TransportFleet_TransportId,
				pair.deliveryInfo.StorageId))
			.ToArray();
	}
	
	public List<int> GetTransportsIds(IEnumerable<TransportFleet_Transport> transports)
	{
		return transports.Select(transport => transport.TransportFleet_TransportId)
			.Distinct()
			.ToList();
	}

	public long[] GetAverageSpeedsByTransport(IEnumerable<TransportFleet_Transport> transports)
	{
		return transports
			.Distinct()
			.Select(transport => (long)transport.AverageSpeed)
			.ToArray();
	}

	public TransportInfo[] GetTransportInfosVector(IEnumerable<TransportFleet_Transport> transports)
	{
		return transports
			.Distinct()
			.Select(transport => new TransportInfo
			{
				Coefficient = transport.CoefficientValue,
				AverageSpeed = transport.AverageSpeed
			})
			.ToArray();
	}

	public decimal[,] GetTransportsToStoragesDistanceMatrix(
		IEnumerable<(TransportFleet_Transport transport, StorageToTransportFleetDistance deliveryInfo)> distanceInformation,
		IList<int> storageIds,
		IList<int> transportIds)
	{
		var result = new decimal[transportIds.Count, storageIds.Count];
		foreach (var transportFleetStorageDistance in distanceInformation)
		{
			var storageIdIndex =
				storageIds.IndexOf(transportFleetStorageDistance.deliveryInfo.StorageId);
			var transportIdIndex =
				transportIds.IndexOf(transportFleetStorageDistance.transport.TransportFleet_TransportId);
			result[transportIdIndex, storageIdIndex] =
				transportFleetStorageDistance.deliveryInfo.Distance!.Value;
		}
		return result;
	}

	public int[] GetConstructionUnitIds(IEnumerable<ConstructionUnitSupplemented> constructionUnitSupplemented)
	{
		return constructionUnitSupplemented
			.Select(constructionUnit => constructionUnit.ConstructionUnitId)
			.OrderBy(constructionUnitId => constructionUnitId)
			.ToArray();
	}

	public List<StorageMaterial> GetConstructionUnitsFromStorages(IEnumerable<int> constructionUnitIds)
	{
		return _data.storagesMaterialsAll
			.Where(storageMaterial => constructionUnitIds.Contains(storageMaterial.ConstructionUnitId))
			.ToList();
	}

	public MaterialParams?[,] GetMaterialParamsMatrix(IReadOnlyList<int> storageIds,
		IReadOnlyList<int> constructionUnitIds,
		IReadOnlyCollection<StorageMaterial> constructionUnitsFromStorage,
		IReadOnlyList<ConstructionUnitSupplemented> constructionUnits)
	{
		var materialParamsMatrix = new MaterialParams?[storageIds.Count, constructionUnitIds.Count];
		for (var i = 0; i < storageIds.Count; i++)
		{
			for (var j = 0; j < constructionUnitIds.Count; j++)
			{
				var constructionUnitFromStorage = constructionUnitsFromStorage
					.FirstOrDefault(cUnit =>
						cUnit!.ConstructionUnitId == constructionUnitIds[j] &&
						cUnit.StorageId == storageIds[i], null);
							
				materialParamsMatrix[i, j] = (constructionUnitFromStorage == null)
					? null
					: new MaterialParams()
					{
						PricePerUnit = constructionUnitFromStorage.PricePerUnit,
						Amount = (decimal)constructionUnits[j].Amount
					};
			}
		}
		return materialParamsMatrix;
	}

	public Dictionary<int, List<ConstructionUnitSupplemented>> GetMaterialsSetsWithConstructionTypes(
		int constructionTypeId)
	{
		var materialSets = _data.materialSets;
		var dbContext = _applicationContext;
		
		return (from mSet in materialSets
				where mSet.ConstructionTypeId == constructionTypeId
				join mSet_cUnit in dbContext.MaterialSet_ConstructionUnit on mSet.MaterialSetId equals mSet_cUnit
					.MaterialSetId
				join cUnit in dbContext.ConstructionUnit on mSet_cUnit.ConstructionUnitId equals cUnit
					.ConstructionUnitId
				join cUnitType in dbContext.ConstructionUnitType on cUnit.ConstructionUnitTypeId equals cUnitType
					.ConstructionUnitTypeId
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
				group => group.Select(g => g.ConstructionUnitWithAmount).ToList());
	}
}