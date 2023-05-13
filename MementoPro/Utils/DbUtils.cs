using System.Linq;
using MementoPro.Database.Models;

namespace MementoPro.Utilities;

public static class DbUtils
{
    public static bool RegisterUser(string login, string password)
    {
        if (IsUserExist(login, password))
            return false;

        var encryptedPassword = password.Encrypt(64);
        var newUser = new User
        {
            Login = login,
            Password = encryptedPassword
        };
        App.Db.Users.Local.Add(newUser);
        App.Db.SaveChanges();
        return true;
    }

    public static bool AuthorizeUser(string login, string password)
    {
        var user = GetUser(login, password);
        if (user == null)
            return false;

        App.CurrentUser = user;
        return true;
    }

    public static bool IsUserExist(string login, string password)
        => GetUser(login, password) != null;

    public static User? GetUser(string login, string password)
    {
        var encryptedPassword = password.Encrypt(64);

        return App.Db.Users.Local.FirstOrDefault(u => u.Login == login && u.Password == encryptedPassword);
    }
}