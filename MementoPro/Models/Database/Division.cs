using System;
using System.Collections.Generic;

namespace MementoPro.Models.Database;

public partial class Division
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
