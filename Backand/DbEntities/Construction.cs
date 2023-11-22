using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backand.DbEntities
{
	public partial class Construction
	{
		public int ConstructionId { get; set; }

		public int ObjectsId { get; set; }

		public int? ConstructionTypeId { get; set; }

		public string ConstructionName { get; set; }

		public BuildState ConstructionStateId { get; set; }

		public virtual ConstructionState? ConstructionState { get; set; }

		public virtual ConstructionType? ConstructionType { get; set; }

		public virtual Objects? Objects { get; set; }

		[JsonIgnore]
		public EntityLink Link { get => new() { Id = ConstructionId, Name = ConstructionName }; }
	}
}
