using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backand.DbEntites
{
    public class Construction
    {
        [Key]
        public int ConstructionId { get; set; }
        public int ObjectsId { get; set; }
        public int ConstructionTypeId { get; set; }
        public string ConstructionName { get; set; }
        public BuildState ConstructionStateId { get; set; }
        [JsonIgnore]
        public EntityLink Link { get => new() { Id = ConstructionId, Name = ConstructionName }; }
        
    }
}
