using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class Storage_ConstructionUnit
{
    public int Storage_ConstructionUnitId { get; set; }

    public int StorageId { get; set; }

    public int ConstructionUnitId { get; set; }

    public decimal PricePerUnit { get; set; }

    public string? DocumentPath { get; set; }

    public string? TablePath { get; set; }

    public virtual ConstructionUnit? ConstructionUnit { get; set; }

    public virtual Storage? Storage { get; set; }
}
