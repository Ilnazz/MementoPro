using System;
using System.Collections.Generic;

namespace MementoPro.Models.Database;

public partial class RequestRejectionReason
{
    public int Id { get; set; }

    public int RequestId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
