using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class ConstructionUnit
{
    public int ConstructionUnitId { get; set; }

    public int ConstructionUnitTypeId { get; set; }

    public string Name { get; set; }

    public int MeasureUnitId { get; set; }

    public virtual ConstructionUnitType? ConstructionUnitType { get; set; }

    public virtual ICollection<MaterialSet_ConstructionUnit> MaterialSet_ConstructionUnits { get; set; } = new List<MaterialSet_ConstructionUnit>();

    public virtual MeasureUnit? MeasureUnit { get; set; }

    public virtual ICollection<Storage_ConstructionUnit> Storage_ConstructionUnits { get; set; } = new List<Storage_ConstructionUnit>();
}
