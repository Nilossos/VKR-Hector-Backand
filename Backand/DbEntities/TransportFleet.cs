using Backand.FrontendEntities;
using NpgsqlTypes;
using System.Text.Json.Serialization;

namespace Backand.DbEntities;

public partial class TransportFleet
{
    public int TransportFleetId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int? CompanyId { get; set; }

    public int RegionId { get; set; }

    [JsonIgnore]
    public NpgsqlPoint Coordinates { get; set; }

    public Spot Spot { get => Coordinates.ToSpot(); }

    public virtual Company? Company { get; set; }

    public virtual Region? Region { get; set; }

    public virtual ICollection<TransportFleet_Transport> TransportFleet_Transports { get; set; } = new List<TransportFleet_Transport>();
}
