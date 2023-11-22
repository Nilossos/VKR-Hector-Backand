using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class MeasureUnit
{
    public int MeasureUnitId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ConstructionUnit> ConstructionUnits { get; set; } = new List<ConstructionUnit>();
}
