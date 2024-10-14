using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class Region
{
    public int RegionId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<DeliveryRegion> DeliveryRegions { get; set; } = new List<DeliveryRegion>();

    public virtual ICollection<ObjectEntity> Objects { get; set; } = new List<ObjectEntity>();

    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();

    public virtual ICollection<TransportFleet> TransportFleets { get; set; } = new List<TransportFleet>();
}
