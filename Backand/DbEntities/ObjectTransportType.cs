using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class ObjectTransportType
{
    public int ObjectTransportTypeId { get; set; }

    public int? ObjectId { get; set; }

    public int? TransportTypeId { get; set; }

    public virtual ObjectEntity? Object { get; set; }

    public virtual TransportType? TransportType { get; set; }
}
