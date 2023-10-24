using System.ComponentModel.DataAnnotations;

namespace Backand.DbEntites
{
    public class ConstructionType
    {
        [Key]
        public int ConstructionTypeId { get; set; }
        public string Name { get; set; }
    }
}
