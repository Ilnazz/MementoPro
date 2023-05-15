using System.Linq;

namespace MementoPro.Database.Models;

public partial class User
{
    public string Name
    {
        get
        {
            if (IsEmployee() == false)
                return Login;

            var employee = Employees.First();

            var prefix = employee.IsGeneralDepartmentEmployee ? "Сотрудник общего отделения" : "Сотрудник";
            return $"{prefix}: {employee.LastName} {employee.FirstName} {employee.Patronymic}";
        }
    }

    public Employee? Employee => Employees.FirstOrDefault();

    public bool IsEmployee() => Employees.Any() == true;

    public bool IsGuest() => Employees.Any() == false;
}