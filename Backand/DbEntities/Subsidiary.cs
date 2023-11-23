
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backand.DbEntities
{
	public partial class Subsidiary
	{
		public int SubsidiaryId { get; set; }


		public string? Name { get; set; }
		[JsonIgnore]
		public virtual ICollection<Mine> Mines { get; set; } = new List<Mine>();
	}
}
