using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backand.DbEntities.ConstructionSpace;

namespace Backand.DbEntities
{
    public partial class ConstructionType
	{
		public int ConstructionTypeId { get; set; }

		public string Name { get; set; }

		public string? DocumentPath { get; set; }
		[JsonIgnore]

		public virtual ICollection<Construction> Constructions { get; set; } = new List<Construction>();
		[JsonIgnore]
		public virtual ICollection<MaterialSet> MaterialSets { get; set; } = new List<MaterialSet>();
	}
}