using Backand.FrontendEntities.Links;
using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class TransportMode
{
    public int TransportModeId { get; set; }

    public string Name { get; set; } = null!;

    public int? TransportTypeId { get; set; }

    public virtual TransportType? TransportType { get; set; }
    public EntityLink Link { get => new() { Id = TransportModeId, Name = Name }; }

    public virtual ICollection<Transport> Transports { get; set; } = new List<Transport>();
}
