using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class ConstructionUnitType
{
    public int ConstructionUnitTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ConstructionUnit> ConstructionUnits { get; set; } = new List<ConstructionUnit>();
}
