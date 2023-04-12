using System;
using System.Collections.Generic;

namespace MementoPro.Models.Database;

public partial class Request
{
    public int Id { get; set; }

    public int RequestTypeId { get; set; }

    public int RequestStatusId { get; set; }

    public int DivisionId { get; set; }

    public DateTime DesiredStartDate { get; set; }

    public DateTime DesiredExpirationDate { get; set; }

    public string VisitPurpose { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual ICollection<RequestRejectionReason> RequestRejectionReasons { get; } = new List<RequestRejectionReason>();

    public virtual RequestStatus RequestStatus { get; set; } = null!;

    public virtual RequestType RequestType { get; set; } = null!;

    public virtual User? User { get; set; }

    public virtual ICollection<Visitor> Visitors { get; } = new List<Visitor>();
}
