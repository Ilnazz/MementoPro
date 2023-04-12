using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Utilities;
using MementoPro.Views.Windows;
using Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace MementoPro.ViewModels;

public partial class AuthWindowModel : ObservableValidator
{
    [NotifyCanExecuteChangedFor(nameof(AuthorizeCommand))]
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    public string? _login;

    [NotifyCanExecuteChangedFor(nameof(AuthorizeCommand))]
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    public string? _password;

    [RelayCommand]
    private void Authorize(Window window)
    {
        if (HasErrors)
        {
            MessageBox.Show(string.Join("\n",
                GetErrors().Select(e => e.ErrorMessage)));

            return;
        }

        if (DbUtils.AuthorizeUser(Login!, Password!) == false)
        {
            MessageBox.Show("User not found.");
            return;
        }

        Session.MainWindowInstance = new MainWindow();
        Session.MainWindowInstance.Show();

        window.Close();
    }
    private bool CanAuthorize() => HasErrors == false;

    [RelayCommand]
    private void GoToRegWindow(Window window)
    {
        new RegWindow().Show();
        window.Close();
    }
}
