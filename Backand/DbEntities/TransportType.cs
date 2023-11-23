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

    public virtual ICollection<ObjectsTransportType> ObjectsTransportTypes { get; set; } = new List<ObjectsTransportType>();

    public virtual ICollection<TransportMode> TransportModes { get; set; } = new List<TransportMode>();
}
