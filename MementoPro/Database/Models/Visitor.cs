using System;
using System.Collections.Generic;

namespace MementoPro.Database.Models;

public partial class Visitor
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? OrganizationId { get; set; }

    public string Note { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public byte[] PassportScan { get; set; } = null!;

    public string PassportScanFileName { get; set; } = null!;

    public virtual Organization? Organization { get; set; }

    public virtual ICollection<Request> Requests { get; } = new List<Request>();
}
