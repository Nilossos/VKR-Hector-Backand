using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class TransportFleet_Transport
{
    public int TransportFleet_TransportId { get; set; }

    public int TransportFleetId { get; set; }

    public int TransportId { get; set; }

    public int AverageSpeed { get; set; }

    public int? CoefficientTypeId { get; set; }

    public float CoefficientValue { get; set; }

    public virtual CoefficientType? CoefficientType { get; set; }

    public virtual ICollection<DeliveryRegion> DeliveryRegions { get; set; } = new List<DeliveryRegion>();

    public virtual Transport? Transport { get; set; }

    public virtual TransportFleet? TransportFleet { get; set; }
}
