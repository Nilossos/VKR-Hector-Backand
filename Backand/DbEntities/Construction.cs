using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backand.DbEntities
{
	public partial class Construction
	{
		public int ConstructionId { get; set; }

		public int ObjectsId { get; set; }
		[JsonIgnore]
		public int ConstructionTypeId { get; set; }

		public string ConstructionName { get; set; }
		[JsonIgnore]
		public BuildState ConstructionStateId { get; set; }
        public ConstructionState? ConstructionState { get; set; }
		
		public ConstructionType? ConstructionType { get; set; }
		[JsonIgnore]
		public Objects? Object { get; set; }

		[JsonIgnore]
		public EntityLink Link { get => new() { Id = ConstructionId, Name = ConstructionName }; }
	}
}
