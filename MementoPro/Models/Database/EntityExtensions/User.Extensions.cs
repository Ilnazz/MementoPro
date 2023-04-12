using Models;
using System.Linq;

namespace MementoPro.Models.Database;

public partial class User
{
    public string Name
    {
        get
        {
            if (IsGuest())
                return "Гость";

            var employee = Employees.First();

            return $"{employee.FirstName} {employee.LastName} {employee.Patronymic}";
        }
    }

    public bool IsEmployee() => Employees.Any() == true;

    public bool IsGuest() => Employees.Any() == false;
}