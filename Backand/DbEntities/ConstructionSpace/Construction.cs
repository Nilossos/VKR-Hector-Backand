using Backand.DbEntites;
using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using Backand.FrontendEntities.Requests;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backand.DbEntities.ConstructionSpace
{
    [JsonConverter(typeof(ConstructionSerializer))]
    public partial class Construction
    {
        public int ConstructionId { get; set; } = 0;

        public int ObjectsId { get; set; }

        public int ConstructionTypeId { get; set; }
        [JsonIgnore]
        public ConstructionType? ConstructionType { get; set; }
        public string ConstructionName { get; set; }
        public BuildState ConstructionStateId { get; set; }
        [JsonIgnore]
        public ConstructionState? ConstructionState { get; set; }
        [ForeignKey(nameof(BuildWay))]
        public BuildWay? BuildWayId { get; set; }
        [JsonIgnore]
        public ConstructionUnitType? BuildWay { get; set; }

        [JsonIgnore]
        public Objects? Object { get; set; }

        [JsonIgnore]
        public EntityLink Link { get => new() { Id = ConstructionId, Name = ConstructionName }; }
    }
}
