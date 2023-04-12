using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Models.Database;
using MementoPro.Utilities;
using MementoPro.Views.Windows;
using Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace MementoPro.ViewModels;
public partial class RegWindowModel : ObservableValidator
{
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    [ObservableProperty]
    public string? _login;

    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    [ObservableProperty]
    [CustomValidation(typeof(RegWindowModel), nameof(ValidatePassword))]
    public string? _password;

    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    [ObservableProperty]
    [CustomValidation(typeof(RegWindowModel), nameof(ValidateConfirmationPassword))]
    public string? _confirmationPassword;

    [RelayCommand]
    private void Register(Window window)
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            MessageBox.Show(string.Join("\n",
                GetErrors().Select(e => e.ErrorMessage)));
            return;
        }

        if (DbUtils.RegisterUser(Login!, Password!) == false)
        {
            MessageBox.Show("Пользователь с таким логином уже зарегистрирован.");
            return;
        }

        new AuthWindow().Show();
        window.Close();
    }

    public static ValidationResult ValidatePassword(string password, ValidationContext context)
    {
        var instance = context.ObjectInstance as RegWindowModel;

        if (password!.Any(char.IsLower) == false)
            return new("Пароль должен содержать по крайней мере один символ нижнего регистра.");
        else if (password!.Any(char.IsUpper) == false)
            return new("Пароль должен содержать по крайней мере один символ верхнего регистра.");
        else if (password!.Any(char.IsDigit) == false)
            return new("Пароль должен содержать по крайней мере одну цифру.");
        else if (password!.Any("~`!@#$%^&*()_+№;:?,.".Contains) == false)
            return new("Пароль должен содержать по крайней мере один специальный символ из набора ~`!@#$%^&*()_+№;:?,.");

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidateConfirmationPassword(string confirmationPassword, ValidationContext context)
    {
        var instance = context.ObjectInstance as RegWindowModel;

        if (confirmationPassword != instance!.Password)
            return new("Пароли должны совпадать.");

        return ValidationResult.Success!;
    }
}