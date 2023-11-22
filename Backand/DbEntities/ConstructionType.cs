using Backand.DbEntities;
using System.ComponentModel.DataAnnotations;

namespace Backand.DbEntities
{
	public partial class ConstructionType
	{
		public int ConstructionTypeId { get; set; }

		public string Name { get; set; }

		public string? DocumentPath { get; set; }

		public virtual ICollection<Construction> Constructions { get; set; } = new List<Construction>();

		public virtual ICollection<MaterialSet> MaterialSets { get; set; } = new List<MaterialSet>();
	}
}