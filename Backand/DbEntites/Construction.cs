using System.ComponentModel.DataAnnotations;

namespace Backand.DbEntites
{
    public class Construction
    {
        [Key]
        public int ConstructionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Boolean IsWorkshop { get; set; }
    }
}
