using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;
using MementoPro.Views;
using Microsoft.Win32;

namespace MementoPro.ViewModels;

public partial class VisitorVM : ViewModelBase
{
    #region [ Properties ]

    public VisitorDocumentsVM VisitorDocumentsVM { get; init; }

    public Visitor Visitor { get; init; }

    [ObservableProperty]
    private bool _canEditPhoto;

    [RegularExpression("[a-zA-Zа-яА-Я]*", ErrorMessage = "Должно содержать только буквы")]
    [Required(ErrorMessage = "Необходимо ввести имя")]
    public string FirstName
    {
        get => Visitor.FirstName;
        set
        {
            ValidateProperty(value);

            Visitor.FirstName = value;
            OnPropertyChanged();
        }
    }

    [RegularExpression("[a-zA-Zа-яА-Я]*", ErrorMessage = "Должно содержать только буквы")]
    [Required(ErrorMessage = "Необходимо ввести фамилию")]
    public string LastName
    {
        get => Visitor.LastName;
        set
        {
            ValidateProperty(value);

            Visitor.LastName = value;
            OnPropertyChanged();
        }
    }

    [RegularExpression("[a-zA-Zа-яА-Я]*", ErrorMessage = "Должно содержать только буквы")]
    public string? Patronymic
    {
        get => Visitor.Patronymic;
        set
        {
            ValidateProperty(value);

            Visitor.Patronymic = value;
            OnPropertyChanged();
        }
    }

    [CustomValidation(typeof(VisitorVM), nameof(ValidatePhone))]
    public string? Phone
    {
        get => Visitor.Phone;
        set
        {
            Visitor.Phone = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    [EmailAddress(ErrorMessage = "Введите верный адрес электронной почты")]
    public string Email
    {
        get => Visitor.Email;
        set
        {
            ValidateProperty(value);

            Visitor.Email = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<Organization> _organizations;
    public IEnumerable<Organization> Organizations => _organizations ??= App.Db.Organizations.Local.Prepend(new Organization { Name = "Нет" });

    public Organization? Organization
    {
        get => Visitor.Organization;
        set
        {
            Visitor.Organization = value;
            OnPropertyChanged();

            if (value?.Name == "Нет")
                Visitor.Organization = null;
        }
    }

    public string? Note
    {
        get => Visitor.Note;
        set
        {
            ValidateProperty(value);

            Visitor.Note = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    [CustomValidation(typeof(VisitorVM), nameof(ValidateBirthDate))]
    public DateTime BirthDate
    {
        get => Visitor.BirthDate;
        set
        {
            ValidateProperty(value);

            Visitor.BirthDate = value;
            OnPropertyChanged();
        }
    }

    [MinLength(4, ErrorMessage = "Серия паспорта должна состоять из четырёх цифр")]
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"\d+", ErrorMessage = "Можно вводить только цифры")]
    public string PassportSeries
    {
        get => Visitor.PassportSeries;
        set
        {
            ValidateProperty(value);

            Visitor.PassportSeries = value;
            OnPropertyChanged();
        }
    }

    [MinLength(6, ErrorMessage = "Номер паспорта должна состоять из шести цифр")]
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"\d+", ErrorMessage = "Можно вводить только цифры")]
    public string PassportNumber
    {
        get => Visitor.PassportNumber;
        set
        {
            ValidateProperty(value);

            Visitor.PassportNumber = value;
            OnPropertyChanged();
        }
    }

    public byte[]? Photo
    {
        get => Visitor.Photo;
        set
        {
            Visitor.Photo = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region [ Validators ]

    public static ValidationResult ValidateBirthDate(DateTime birthDate, ValidationContext _)
    {
        var ageInYears = DateTime.Now.Year - birthDate.Date.Year;
        if (ageInYears < 16)
            return new("Возраст посетителя должен быть не моложе 16 лет");

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidatePhone(string phone, ValidationContext _)
    {
        if (phone?.Length > 0 && phone?.Length != 17)
            return new("Введите номер телефона целиком");

        return ValidationResult.Success!;
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void UploadPhoto()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "All image files|*.jpg;*.jpeg;*.png|JPG files (*.jpg)|*.jpg|JPEG file (*.jpeg)|*.jpeg|PNG files (*.png)|*.png",
            CheckFileExists = true
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        var filePath = openFileDialog.FileName;
        var image = File.ReadAllBytes(filePath);

        Photo = image;

        ClearPhotoCommand.NotifyCanExecuteChanged();
    }



    [RelayCommand(CanExecute = nameof(CanClearPhoto))]
    private void ClearPhoto()
    {
        Photo = null;

        ClearPhotoCommand.NotifyCanExecuteChanged();
    }
    private bool CanClearPhoto() => Photo != null;

    #endregion

    #region [ Public methods ]

    public void ResetData()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Patronymic = string.Empty;
        Phone = string.Empty;
        Email = string.Empty;
        Note = string.Empty;
        PassportSeries = string.Empty;
        PassportNumber = string.Empty;

        Organization = Organizations.First();
        BirthDate = new DateTime(DateTime.Now.Year - 16, DateTime.Now.Month, DateTime.Now.Day);

        ClearPhoto();

        ClearErrors();

        VisitorDocumentsVM.ResetData();
    }

    public bool Validate()
    {
        ValidateAllProperties();

        return VisitorDocumentsVM.Validate() && (HasErrors == false);
    }

    #endregion

    #region [ Setup ]

    public VisitorVM(Visitor visitor)
    {
        Visitor = visitor;
        VisitorDocumentsVM = new VisitorDocumentsVM(visitor);
    }

    #endregion
}
