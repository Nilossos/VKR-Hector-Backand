using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class Transport
{
    public int TransportId { get; set; }

    public int? TransportModeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TransportFleet_Transport> TransportFleet_Transports { get; set; } = new List<TransportFleet_Transport>();

    public virtual TransportMode? TransportMode { get; set; }
}
