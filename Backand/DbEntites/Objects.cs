using System.ComponentModel.DataAnnotations;

namespace Backand.DbEntites
{
    public class Objects
    {
        [Key]
        public int ObjectsId { get; set; }
        public string Name { get; set; }
        public NpgsqlTypes.NpgsqlPoint Coordinates { get; set; }
        public int MineId { get; set; }
    }
}
