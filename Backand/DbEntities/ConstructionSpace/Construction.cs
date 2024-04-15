using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backand.DbEntities.ConstructionSpace
{
    public partial class Construction
    {
        public int ConstructionId { get; set; } = 0;

        public int ObjectsId { get; set; }

        public int ConstructionTypeId { get; set; }
        [JsonIgnore]
        public virtual ConstructionType? ConstructionType { get; set; }
        public string ConstructionName { get; set; }
        public BuildState ConstructionStateId { get; set; }
        [JsonIgnore]
        public virtual ConstructionState? ConstructionState { get; set; }


        [JsonIgnore]
        public virtual Objects? Object { get; set; }

        [JsonIgnore]
        public EntityLink Link { get => new() { Id = ConstructionId, Name = ConstructionName }; }
    }
}
