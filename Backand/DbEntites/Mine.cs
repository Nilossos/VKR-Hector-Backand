using Backand.FrontendEntities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backand.DbEntites
{
    public class Mine
    {
        [Key]
        public int MineId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public NpgsqlTypes.NpgsqlPoint Coordinates { get; set; }
        public Spot Center { get => Coordinates.ToSpot(); }
        public int SubsidiaryId { get; set; }
        [JsonIgnore]
        public EntityLink Link { get => new() { Id = MineId, Name = Name}; }
        //private Spot _center;
    }
}
