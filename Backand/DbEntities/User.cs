using System;
using System.Collections.Generic;

namespace Backand.DbEntities;

public partial class User
{
    public int UserId { get; set; }

    public int? UserTypeId { get; set; }

    public int? SubsidiaryId { get; set; }

    public string Surname { get; set; }

    public string FirstName { get; set; }

    public string? Patronymic { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime BirthDate { get; set; }

    public string? PhotoPath { get; set; }

    public string? Token { get; set; }

    public virtual Subsidiary? Subsidiary { get; set; }

    public virtual UserType? UserType { get; set; }
}
