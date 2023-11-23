using System;
using System.Collections.Generic;

namespace Backand.DbEntities
{
    public partial class DeliveryRegion
    {
        public int DeliveryRegionId { get; set; }

        public int? TransportFleet_TransportId { get; set; }

        public int? RegionId { get; set; }

        public virtual Region? Region { get; set; }

        public virtual TransportFleet_Transport? TransportFleet_Transport { get; set; }
    }
}
