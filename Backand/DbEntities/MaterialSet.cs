using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class MaterialSet
{
    public int MaterialSetId { get; set; }

    public int? ConstructionTypeId { get; set; }

    public virtual ConstructionType? ConstructionType { get; set; }

    public virtual ICollection<MaterialSet_ConstructionUnit> MaterialSet_ConstructionUnits { get; set; } = new List<MaterialSet_ConstructionUnit>();
}
