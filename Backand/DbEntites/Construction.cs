using Backand.FrontendEntities;
using System.ComponentModel.DataAnnotations;

namespace Backand.DbEntites
{
    public class Construction
    {
        [Key]
        public int ConstructionId { get; set; }
        public int ConstructionTypeId { get; set; }
        public string Description { get; set; }
        public Boolean IsWorkshop { get; set; }
        
    }
}
