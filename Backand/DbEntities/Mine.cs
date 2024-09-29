using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backand.DbEntities
{
	public partial class Mine
	{
		public int MineId { get; set; }

		public string Name { get; set; }

		[JsonIgnore]
		public NpgsqlPoint Coordinates { get; set; }

		public Spot Center { get => Coordinates.ToSpot(); }

		public string? DocumentPath { get; set; }

		[JsonIgnore]
		public int? SubsidiaryId { get; set; }
		[JsonIgnore]
		public MineLink Link { get => new() { 
			Id = MineId, Name = Name, Coordination = Center, Objects = Objects.Select(o=>o.MapLink).ToArray()
		};}
		[JsonIgnore]
		public virtual ICollection<Objects> Objects { get; set; } = new List<Objects>();

        public virtual Subsidiary? Subsidiary { get; set; }
    }
}
