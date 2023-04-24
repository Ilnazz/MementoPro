using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Utilities;
using MementoPro.ViewModels.Base;
using MementoPro.Views.Windows;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace MementoPro.ViewModels;
public sealed partial class RegVM : WindowViewModelBase
{
    #region [ Properties ]

    [Required(ErrorMessage = "Обязательное поле.")]
    [MinLength(8, ErrorMessage = "Не менее 8 символов.")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    public string? _login;

    [Required(ErrorMessage = "Обязательное поле.")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [CustomValidation(typeof(RegVM), nameof(ValidatePassword))]
    public string? _password;

    [Required(ErrorMessage = "Обязательное поле.")]
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    [CustomValidation(typeof(RegVM), nameof(ValidateConfirmationPassword))]
    public string? _confirmationPassword;

    #endregion

    #region [ Commands ]

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private void Register()
    {
        ValidateAllProperties();

        if (DbUtils.RegisterUser(Login!, Password!) == false)
        {
            MessageBox.Show("Пользователь с таким логином уже зарегистрирован.");
            return;
        }

        DbUtils.AuthorizeUser(Login!, Password!);

        new WindowView(new MainVM()).Show();
        CloseWindow();
    }
    private bool CanRegister() => HasErrors == false;

    [RelayCommand]
    private void GoToAuthWindow()
    {
        new WindowView(new AuthVM()).Show();
        CloseWindow();
    }

    #endregion

    #region [ Specific password validation functions ]

    public static ValidationResult ValidatePassword(string password, ValidationContext context)
    {
        var instance = context.ObjectInstance as RegVM;

        if (password.Length < 8)
            return new("Не менее 8 символов.");
        if (password!.Any(char.IsLower) == false)
            return new("Мин. 1 символ нижнего регистра.");
        else if (password!.Any(char.IsUpper) == false)
            return new("Мин. 1 символ верхнего регистра.");
        else if (password!.Any(char.IsDigit) == false)
            return new("Мин. 1 цифра.");
        else if (password!.Any("~`!@#$%^&*()_+№;:?,.".Contains) == false)
            return new("Мин. 1 символ из ~`!@#$%^&*()_+№;:?,.");

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidateConfirmationPassword(string confirmationPassword, ValidationContext context)
    {
        var instance = context.ObjectInstance as RegVM;

        if (confirmationPassword != instance!.Password)
            return new("Пароли должны совпадать.");

        return ValidationResult.Success!;
    }

    #endregion
}