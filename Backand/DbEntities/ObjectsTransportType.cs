using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class ObjectsTransportType
{
    public int ObjectsTransportTypeId { get; set; }

    public int? ObjectsId { get; set; }

    public int? TransportTypeId { get; set; }

    public virtual Objects? Objects { get; set; }

    public virtual TransportType? TransportType { get; set; }
}
