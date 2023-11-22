using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class MaterialSet_ConstructionUnit
{
    public int MaterialSet_ConstructionUnitId { get; set; }

    public int? MaterialSetId { get; set; }

    public int? ConstructionUnitId { get; set; }

    public float Amount { get; set; }

    public virtual ConstructionUnit? ConstructionUnit { get; set; }

    public virtual MaterialSet? MaterialSet { get; set; }
}
