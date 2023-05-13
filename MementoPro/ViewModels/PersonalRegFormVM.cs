using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public sealed partial class PersonalRegFormVM : ViewModelBase
{
    #region [ Properties ]

    [Required(ErrorMessage = "Обязательное поле")]
    [CustomValidation(typeof(PersonalRegFormVM), nameof(ValidateDesiredStartDate))]
    public DateTime DesiredStartDate
    {
        get => _request.DesiredStartDate;
        set
        {
            ValidateProperty(value);

            _request.DesiredStartDate = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    [CustomValidation(typeof(PersonalRegFormVM), nameof(ValidateDesiredExpirationDate))]
    public DateTime DesiredExpirationDate
    {
        get => _request.DesiredExpirationDate;
        set
        {
            ValidateProperty(value);

            _request.DesiredExpirationDate = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<VisitPurpose> _visitPurposes;
    public IEnumerable<VisitPurpose> VisitPurposes => _visitPurposes ??= App.Db.VisitPurposes.Local.ToList();

    public VisitPurpose VisitPurpose
    {
        get => _request.VisitPurpose;
        set
        {
            _request.VisitPurpose = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<Division> _divisions;
    public IEnumerable<Division> Divisions => _divisions ??= App.Db.Divisions.Local.ToList();

    public Division Division
    {
        get => _request.Division;
        set
        {
            _request.Division = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Employees));
        }
    }

    public IEnumerable<Employee> Employees => Division.Employees;

    public Employee Employee
    {
        get => _request.Employee;
        set
        {
            _request.Employee = value;
            OnPropertyChanged();
        }
    }

    #region [ Visitor properties ]

    [Required(ErrorMessage = "Необходимо ввести имя")]
    public string VisitorFirstName
    {
        get => _visitor.FirstName;
        set
        {
            ValidateProperty(value);

            _visitor.FirstName = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Необходимо ввести фамилию")]
    public string VisitorLastName
    {
        get => _visitor.LastName;
        set
        {
            ValidateProperty(value);

            _visitor.LastName = value;
            OnPropertyChanged();
        }
    }

    public string? VisitorPatronymic
    {
        get => _visitor.Patronymic;
        set
        {
            _visitor.Patronymic = value;
            OnPropertyChanged();
        }
    }

    public string? VisitorPhone
    {
        get => _visitor.Phone;
        set
        {
            _visitor.Phone = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    [CustomValidation(typeof(PersonalRegFormVM), nameof(ValidateEmail))]
    public string VisitorEmail
    {
        get => _visitor.Email;
        set
        {
            ValidateProperty(value);

            _visitor.Email = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<Organization> _organizations;
    public IEnumerable<Organization> Organizations => _organizations ??= App.Db.Organizations.Local.Prepend(new Organization { Name = "Нет" });

    public Organization? VisitorOrganization
    {
        get => _visitor.Organization;
        set
        {
            _visitor.Organization = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Необходимо ввести примечание")]
    public string VisitorNote
    {
        get => _visitor.Note;
        set
        {
            ValidateProperty(value);

            _visitor.Note = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    [CustomValidation(typeof(PersonalRegFormVM), nameof(ValidateVisitorBirthDate))]
    public DateTime VisitorBirthDate
    {
        get => _visitor.BirthDate;
        set
        {
            ValidateProperty(value);

            _visitor.BirthDate = value;
            OnPropertyChanged();
        }
    }

    [MinLength(4, ErrorMessage = "Серия паспорта должна состоять из четырёх цифр")]
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"\d+", ErrorMessage = "Можно вводить только цифры")]
    public string VisitorPassportSeries
    {
        get => _visitor.PassportSeries;
        set
        {
            ValidateProperty(value);

            _visitor.PassportSeries = value;
            OnPropertyChanged();
        }
    }

    [MinLength(6, ErrorMessage = "Номер паспорта должна состоять из шести цифр")]
    [Required(ErrorMessage = "Обязательное поле")]
    [RegularExpression(@"\d+", ErrorMessage = "Можно вводить только цифры")]
    public string VisitorPassportNumber
    {
        get => _visitor.PassportNumber;
        set
        {
            ValidateProperty(value);

            _visitor.PassportNumber = value;
            OnPropertyChanged();
        }
    }

    [CustomValidation(typeof(PersonalRegFormVM), nameof(ValidatePhone))]
    public byte[]? VisitorPhoto
    {
        get => _visitor.Photo;
        set
        {
            _visitor.Photo = value;
            OnPropertyChanged();
        }
    }

    public string VisitorPassportScanFileName
    {
        get => _visitor.PassportScanFileName;
        set
        {
            _visitor.PassportScanFileName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsVisitorPassportScanAttached));
        }
    }

    public bool IsVisitorPassportScanAttached => string.IsNullOrEmpty(VisitorPassportScanFileName) == false;

    #endregion

    #region [ Validators ]

    public static ValidationResult ValidateDesiredStartDate(DateTime desiredStartDate, ValidationContext _)
    {
        if ((desiredStartDate - DateTime.Now.Date).Days < 1)
            return new("Дата начала должна быть не ранее следующего дня");
        else if ((desiredStartDate - DateTime.Now.Date).Days > 15)
            return new("Дата начала должна отстоять от текущей даты не более чем на 15 дней");

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidateDesiredExpirationDate(DateTime desiredExpirationDate, ValidationContext context)
    {
        var instance = (PersonalRegFormVM)context.ObjectInstance;

        var desiredStartDate = instance.DesiredStartDate;

        if (desiredExpirationDate.Date < desiredStartDate.Date)
            return new("Дата окончания не может быть раньше даты начала.");
        else if ((desiredExpirationDate - desiredStartDate).Days > 15)
            return new("Дата окончания должна отстоять от даты начала не более чем на 15 дней");

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidateVisitorBirthDate(DateTime birthDate, ValidationContext context)
    {
        var ageInYears = DateTime.Now.Year - birthDate.Date.Year;
        if (ageInYears < 16)
            return new("Возраст посетителя должен быть не моложе 16 лет");

        return ValidationResult.Success!;
    }

    public static ValidationResult ValidateEmail(string emailStr, ValidationContext context)
    {
        try
        {
            new MailAddress(emailStr);
            return ValidationResult.Success!;
        }
        catch (FormatException)
        {
            return new("Введите верный адрес электронной почты");
        }
    }

    public static ValidationResult ValidatePhone(string phone, ValidationContext context)
    {
        if (phone?.Length > 0 && phone?.Length != 17)
            return new("Введите номер телефона целиком");

        return ValidationResult.Success!;
    }

    #endregion

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void UploadVisitorPhoto()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "All image files|*.jpg;*.jpeg;*.png|JPG files (*.jpg)|*.jpg|JPEG file (*.jpeg)|*.jpeg|PNG files (*.png)|*.png",
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        //Get the path of specified file
        var filePath = openFileDialog.FileName;
        var image = File.ReadAllBytes(filePath);

        VisitorPhoto = image;
        ClearVisitorPhotoCommand.NotifyCanExecuteChanged();
    }



    [RelayCommand(CanExecute = nameof(CanClearVisitorPhoto))]
    private void ClearVisitorPhoto()
    {
        VisitorPhoto = null;
        ClearVisitorPhotoCommand.NotifyCanExecuteChanged();
    }

    private bool CanClearVisitorPhoto() => VisitorPhoto != null;



    [RelayCommand(CanExecute = nameof(CanAttachPassportScan))]
    private void AttachPassportScan()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "PDF files|*.pdf"
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        //Get the path of specified file
        var filePath = openFileDialog.FileName;
        var file = File.ReadAllBytes(filePath);

        VisitorPassportScanFileName = Path.GetFileName(openFileDialog.FileName);
        _visitor.PassportScan = file;

        AttachPassportScanCommand.NotifyCanExecuteChanged();
    }
    private bool CanAttachPassportScan() => _visitor.PassportScan == null;



    [RelayCommand]
    private void ClearVisitorPassportScan()
    {
        VisitorPassportScanFileName = null;
        _visitor.PassportScan = null;

        AttachPassportScanCommand.NotifyCanExecuteChanged();
    }

    
    
    [RelayCommand]
    private void ResetFormData()
    {
        DesiredStartDate = DateTime.Now.AddDays(1);
        DesiredExpirationDate = DateTime.Now.AddDays(16);
        VisitPurpose = VisitPurposes.First();
        Division = Divisions.First();
        Employee = Employees.First();

        VisitorFirstName = "";
        VisitorLastName = "";
        VisitorPatronymic = "";
        VisitorPhone = "";
        VisitorEmail = "";
        VisitorNote = "";
        VisitorPassportSeries = "";
        VisitorPassportNumber = "";

        VisitorOrganization = Organizations.First();
        VisitorBirthDate = new DateTime(DateTime.Now.Year - 16, DateTime.Now.Month, DateTime.Now.Day);

        ClearVisitorPhoto();
        ClearVisitorPassportScan();

        ClearErrors();
    }



    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private void Submit()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        if (_visitor.PassportScan == null)
        {
            MessageBox.Show("Необходимо прикрепить скан паспорта");
            return;
        }

        App.Db.SaveChanges();

        _mainVM.GoBack();
    }
    private bool CanSubmit() => HasErrors == false; 

    #endregion

    #region [ Fields ]

    private Request _request = null!;

    private Visitor _visitor = null!;

    private MainVM _mainVM = null!;

    #endregion

    #region [ Setup ]

    public PersonalRegFormVM(MainVM mainVM)
    {
        Title = "Форма регистрации на посещение мероприятия";

        _mainVM = mainVM;

        _visitor = new();

        _request = new Request
        {
            RequestTypeId = (int)DataTypes.Enums.RequestType.Personal,
            RequestStatusId = (int)DataTypes.Enums.RequestStatus.OnInspection,
        };
        _request.Visitors.Add(_visitor);

        ResetFormData();
    }

    #endregion
}