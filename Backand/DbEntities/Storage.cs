using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class Storage
{
    public int StorageId { get; set; }

    public string Name { get; set; } = null!;

    public int? RegionId { get; set; }

    public string Address { get; set; } = null!;

    public int? ManufacturerId { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual Region? Region { get; set; }

    public virtual ICollection<Storage_ConstructionUnit> Storage_ConstructionUnits { get; set; } = new List<Storage_ConstructionUnit>();
}
