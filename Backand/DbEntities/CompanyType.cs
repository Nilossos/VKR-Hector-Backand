using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class CompanyType
{
    public int CompanyTypeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
