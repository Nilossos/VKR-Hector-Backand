using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class Company
{
    public int CompanyId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Url { get; set; }

    public int CompanyTypeId { get; set; }

    public virtual CompanyType? CompanyType { get; set; }

    public virtual LogisticCompany? LogisticCompany { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<TransportFleet> TransportFleets { get; set; } = new List<TransportFleet>();
}
