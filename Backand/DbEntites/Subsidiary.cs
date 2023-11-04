
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backand.DbEntites
{
    public class Subsidiary
    {
        [Key]
        [Column("SubsidiaryId")]
        public int SudsidiaryId { get; set; }
        public string Name { get; set; }
    }
}
