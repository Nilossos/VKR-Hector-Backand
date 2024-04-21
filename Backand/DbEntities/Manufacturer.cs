namespace Backand.DbEntities;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Company ManufacturerNavigation { get; set; } = null!;

    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();
}
