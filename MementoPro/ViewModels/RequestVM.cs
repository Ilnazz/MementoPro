using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public class RequestVM : ViewModelBase
{
    #region [ Properties ]

    public Request Request { get; init; }

    [Required(ErrorMessage = "Обязательное поле")]
    [CustomValidation(typeof(RequestVM), nameof(ValidateDesiredStartDate))]
    public DateTime DesiredStartDate
    {
        get => Request.DesiredStartDate;
        set
        {
            ValidateProperty(value);

            Request.DesiredStartDate = value;
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Обязательное поле")]
    [CustomValidation(typeof(RequestVM), nameof(ValidateDesiredExpirationDate))]
    public DateTime DesiredExpirationDate
    {
        get => Request.DesiredExpirationDate;
        set
        {
            ValidateProperty(value);

            Request.DesiredExpirationDate = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<VisitPurpose> _visitPurposes;
    public IEnumerable<VisitPurpose> VisitPurposes => _visitPurposes ??= App.Db.VisitPurposes.Local.ToList();

    public VisitPurpose VisitPurpose
    {
        get => Request.VisitPurpose;
        set
        {
            Request.VisitPurpose = value;
            OnPropertyChanged();
        }
    }

    private IEnumerable<Division> _divisions;
    public IEnumerable<Division> Divisions => _divisions ??= App.Db.Divisions.Local.ToList();

    public Division Division
    {
        get => Request.Division;
        set
        {
            Request.Division = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Employees));
        }
    }

    public IEnumerable<Employee> Employees => Division.Employees;

    public Employee Employee
    {
        get => Request.Employee;
        set
        {
            Request.Employee = value;
            OnPropertyChanged();
        }
    }

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
        var instance = (RequestVM)context.ObjectInstance;

        var desiredStartDate = instance.DesiredStartDate;

        if (desiredExpirationDate.Date < desiredStartDate.Date)
            return new("Дата окончания не может быть раньше даты начала.");
        else if ((desiredExpirationDate - desiredStartDate).Days > 15)
            return new("Дата окончания должна отстоять от даты начала не более чем на 15 дней");

        return ValidationResult.Success!;
    }

    #endregion

    #region [ Public methods ]

    public void ResetData()
    {
        DesiredStartDate = DateTime.Now.AddDays(1);
        DesiredExpirationDate = DateTime.Now.AddDays(16);
        VisitPurpose = VisitPurposes.FirstOrDefault();
        Division = Divisions.FirstOrDefault();
        Employee = Employees.FirstOrDefault();

        ClearErrors();
    }

    public bool Validate()
    {
        ValidateAllProperties();
        return HasErrors;
    }

    #endregion

    #region [ Setup ]

    public RequestVM(Request request)
    {
        Request = request;

        ResetData();
    }

    #endregion
}
