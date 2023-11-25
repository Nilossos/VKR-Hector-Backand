using Backand.FrontendEntities.Links;
using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class ConstructionUnitType
{
    public int ConstructionUnitTypeId { get; set; }

    public string Name { get; set; } = null!;
    public EntityLink Link { get => new() { Id=ConstructionUnitTypeId, Name=Name }; }

    public virtual ICollection<ConstructionUnit> ConstructionUnits { get; set; } = new List<ConstructionUnit>();
}
