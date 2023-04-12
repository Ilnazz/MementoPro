using System;
using System.Collections.Generic;

namespace MementoPro.Models.Database;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public int DivisionId { get; set; }

    public int UserId { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
