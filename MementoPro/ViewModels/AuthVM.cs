using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Utilities;
using MementoPro.ViewModels.Base;
using MementoPro.Views.Windows;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace MementoPro.ViewModels;

public sealed partial class AuthVM : WindowViewModelBase
{
    #region [ Properties ]

    [NotifyCanExecuteChangedFor(nameof(AuthorizeCommand))]
    [Required(ErrorMessage = "Обязательное поле.")]
    [MinLength(8, ErrorMessage = "Не менее 8 символов.")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    public string? _login = "_Mazepa_";

    [NotifyCanExecuteChangedFor(nameof(AuthorizeCommand))]
    [Required(ErrorMessage = "Обязательное поле.")]
    [MinLength(8, ErrorMessage = "Не менее 8 символов.")]
    [NotifyDataErrorInfo]
    [ObservableProperty]
    public string? _password = "Mazepa123!";

    #endregion

    #region [ Commands ]

    [RelayCommand(CanExecute = nameof(CanAuthorize))]
    private void Authorize()
    {
        ValidateAllProperties();

        if (DbUtils.AuthorizeUser(Login!, Password!) == false)
        {
            MessageBox.Show("Неверные логин и/или пароль.");
            return;
        }

        new WindowView(new MainVM()).Show();
        CloseWindow();

    }
    private bool CanAuthorize() => HasErrors == false;

    [RelayCommand]
    private void GoToRegWindow()
    {
        new WindowView(new RegVM()).Show();
        CloseWindow();
    }

    #endregion
}
