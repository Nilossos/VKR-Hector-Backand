using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class TransportType
{
    public int TransportTypeId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<ObjectsTransportType> ObjectsTransportTypes { get; set; } = new List<ObjectsTransportType>();

    public virtual ICollection<TransportMode> TransportModes { get; set; } = new List<TransportMode>();
}
