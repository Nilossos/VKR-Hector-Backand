using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backand.DbEntites
{
    public class Objects
    {
        [Key]
        public int ObjectsId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public NpgsqlTypes.NpgsqlPoint Coordinates { get; set; }
        public Spot Spot { get => Coordinates.ToSpot(); }
        public int MineId { get; set; }
        [JsonIgnore]
        public EntityLink Link { get => new() { Id = ObjectsId, Name = this.Name }; }
    }
}
