using Backand.FrontendEntities.Links;
using System;
using System.Collections.Generic;
using Backand.DbEntities.ConstructionSpace;
using Backand.DbEntites;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backand.DbEntities;

public partial class ConstructionUnitType
{
    public BuildWay ConstructionUnitTypeId { get; set; }

    public string Name { get; set; } = null!;
    public EntityLink Link { get => new() { Id=(int)ConstructionUnitTypeId, Name=Name }; }

    public virtual ICollection<ConstructionUnit>? ConstructionUnits { get; set; } = new List<ConstructionUnit>();
    [NotMapped]
    public List<Construction>? Constructions { get; set; } = new();
}
