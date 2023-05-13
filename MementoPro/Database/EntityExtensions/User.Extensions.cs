using System.Linq;

namespace MementoPro.Database.Models;

public partial class User
{
    public string Name
    {
        get
        {
            if (IsGuest())
                return "Гость";

            var employee = Employees.First();
            return $"{employee.LastName} {employee.FirstName} {employee.Patronymic}";
        }
    }

    public Employee? Employee => Employees.FirstOrDefault();

    public bool IsEmployee() => Employees.Any() == true;

    public bool IsGuest() => Employees.Any() == false;
}