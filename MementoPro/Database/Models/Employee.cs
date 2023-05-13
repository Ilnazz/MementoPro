using System;
using System.Collections.Generic;

namespace MementoPro.Database.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Division> Divisions { get; } = new List<Division>();
}
