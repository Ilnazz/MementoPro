using System;
using System.Collections.Generic;

namespace MementoPro.Database.Models;

public partial class EmployeeDivision
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int DivisionId { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
