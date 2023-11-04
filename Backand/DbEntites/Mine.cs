using Backand.FrontendEntities;
using Backand.FrontendEntities.Links;
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
        [JsonIgnore]
        public int SubsidiaryId { get; set; }
        //private Spot _center;
    }
}
