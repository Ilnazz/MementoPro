using MementoPro;
using MementoPro.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Models;
public static class Session
{
    public static readonly MementoProContext Db = new();

    public static User? CurrentUser { get; set; }

    public static MainWindow? MainWindowInstance { get; set; }

    static Session()
    {
        Db.Users.Load();
        Db.Visitors.Load();
        Db.Requests.Load();
        Db.Divisions.Load();
        Db.Employees.Load();
        Db.RequestTypes.Load();
        Db.Organizations.Load();
        Db.RequestStatuses.Load();
        Db.RequestRejectionReasons.Load();
    }
}