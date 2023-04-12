using System;
using System.Collections.Generic;

namespace MementoPro.Models.Database;

public partial class RequestType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
