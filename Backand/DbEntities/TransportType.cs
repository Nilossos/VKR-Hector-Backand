using Backand.FrontendEntities.Links;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backand.DbEntities;

public partial class TransportType
{
    public int TransportTypeId { get; set; }

    public string Name { get; set; }
    [JsonIgnore]
    public EntityLink Link { get => new() {Id=TransportTypeId, Name=Name }; }

    public virtual ICollection<ObjectTransportType> ObjectTransportTypes { get; set; } = new List<ObjectTransportType>();

    public virtual ICollection<TransportMode> TransportModes { get; set; } = new List<TransportMode>();
}

public enum TransportTypeValue
{
    Air = 1,
    Ground,
    Water
}
