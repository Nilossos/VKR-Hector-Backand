using Backand.DbEntities.ConstructionSpace;
using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backand.DbEntities
{
    public partial class Objects
	{
		public int ObjectsId { get; set; }

		public string? Name { get; set; }

		[JsonIgnore]
		public NpgsqlPoint Coordinates { get; set; }

		public Spot Spot { get => Coordinates.ToSpot(); }

		public int MineId { get; set; }

		public int RegionId { get; set; }

		public bool ContainsAssemblyShop { get; set; }

		public virtual ICollection<Construction> Constructions { get; set; } = new List<Construction>();

		public virtual Mine? Mine { get; set; }

		public virtual ICollection<ObjectsTransportType> ObjectsTransportTypes { get; set; } = new List<ObjectsTransportType>();

		public virtual Region? Region { get; set; }

		[JsonIgnore]
		public EntityLink Link { get => new() { Id = ObjectsId, Name = Name }; }
		public MapLink MapLink { get => new() { Id = ObjectsId, Name = Name, Coordination = Spot }; }
	}
}
