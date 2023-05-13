using System.Linq;

namespace MementoPro.Database.Models;

public partial class Employee
{
    public override string ToString() => $"{LastName} {FirstName} {Patronymic}";

    public bool IsGeneralDepartmentEmployee => Divisions.Any() == false;
}
