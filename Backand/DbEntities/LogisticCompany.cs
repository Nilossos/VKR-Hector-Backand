using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class LogisticCompany
{
    public int LogisticCompanyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Company LogisticCompanyNavigation { get; set; } = null!;
}
