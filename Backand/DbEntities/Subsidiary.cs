
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backand.DbEntities
{
	public partial class Subsidiary
	{
		public int SubsidiaryId { get; set; }


		public string? Name { get; set; }

		public virtual ICollection<Mine> Mines { get; set; } = new List<Mine>();
	}
}
