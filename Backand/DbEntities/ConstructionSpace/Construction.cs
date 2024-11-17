using Backand.DbEntites;
using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using Backand.FrontendEntities.Requests;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backand.DbEntities.ConstructionSpace
{
    //[JsonConverter(typeof(ConstructionSerializer))]
    public partial class Construction
    {
        [Key]
        public int ConstructionId { get; set; }

        public string ConstructionName { get; set; } = null!;

        public int ConstructionTypeId { get; set; }
        public virtual ConstructionType? ConstructionType { get; set; }

        public BuildState? ConstructionStateId { get; set; }
        [ForeignKey(nameof(ConstructionStateId))]
        public virtual ConstructionState? ConstructionState { get; set; }

        public BuildWay? BuildWayId { get; set; }
        [ForeignKey(nameof(BuildWayId))]
        public virtual ConstructionUnitType? BuildWay { get; set; }

        public int ObjectId { get; set; }
        [ForeignKey(nameof(ObjectId))]
        public virtual ObjectEntity Object { get; set; }

        //[JsonIgnore]
        //public EntityLink Link { get => new() { Id = ConstructionId, Name = ConstructionName }; }
    }
}
