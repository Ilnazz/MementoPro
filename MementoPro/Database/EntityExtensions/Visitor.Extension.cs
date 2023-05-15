namespace MementoPro.Database.Models;

public partial class Visitor
{
    public string FullName => $"{LastName} {FirstName} {Patronymic}";
}
