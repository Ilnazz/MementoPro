﻿using MementoPro.Database.Models;
using MementoPro.Database;
using MementoPro.ViewModels;
using MementoPro.Views.Windows;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MementoPro;

public partial class App : Application
{
    public static readonly DatabaseContext Db = new();

    public static User? CurrentUser { get; set; }

    static App()
    {
        Db.Users.Load();
        Db.Visitors.Load();
        Db.Requests.Load();
        Db.Divisions.Load();
        Db.Employees.Load();
        Db.RequestTypes.Load();
        Db.Organizations.Load();
        Db.VisitPurposes.Load();
        Db.RequestStatuses.Load();
        Db.RequestRejectionReasons.Load();
        Db.EmployeeDivisions.Load();
    }

    private void App_Startup(object sender, StartupEventArgs e)
        => new WindowView(new MainVM()).Show();
}
