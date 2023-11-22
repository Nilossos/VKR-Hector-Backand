using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class CoefficientType
{
    public int CoefficientTypeId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<TransportFleet_Transport> TransportFleet_Transports { get; set; } = new List<TransportFleet_Transport>();
}
