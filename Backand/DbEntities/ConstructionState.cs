using System.Text.Json.Serialization;
using Backand.DbEntities.ConstructionSpace;

namespace Backand.DbEntities
{
    public enum BuildState
    {
        Planned=1,
        Verified,
        Building,
        Built
    }
    public class ConstructionState
    {
        public BuildState ConstructionStateId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]

		public virtual ICollection<Construction> Constructions { get; set; } = new List<Construction>();
	}
}
