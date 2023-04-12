using System;
using System.Collections.Generic;

namespace MementoPro.Models.Database;

public partial class Organization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Visitor> Visitors { get; } = new List<Visitor>();
}
