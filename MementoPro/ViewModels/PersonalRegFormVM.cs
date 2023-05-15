using System.Windows;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public sealed partial class PersonalRegFormVM : ViewModelBase
{
    #region [ Properties ]

    public VisitorVM VisitorVM { get; init; }

    public RequestVM RequestVM { get; init; }

    #endregion

    #region [ Commands ]

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private void Submit()
    {
        if (VisitorVM.Validate() == false || RequestVM.Validate() == false)
            return;

        App.Db.Requests.Add(RequestVM.Request);
        App.Db.Visitors.Add(VisitorVM.Visitor);
        App.Db.SaveChanges();

        MessageBox.Show("Заявка зарегистрирована", "Сообщение");

        MainVM.Instance.GoBack();
    }
    private bool CanSubmit() => HasErrors == false;

    [RelayCommand]
    private void ResetData()
    {
        VisitorVM.ResetData();
        RequestVM.ResetData();
    }

    #endregion

    #region [ Setup ]

    public PersonalRegFormVM()
    {
        Title = "Индивидуальная форма регистрации на посещение мероприятия";

        var request = new Request
        {
            User = App.CurrentUser,
            RequestTypeId = (int)DataTypes.Enums.RequestType.Personal,
            RequestStatusId = (int)DataTypes.Enums.RequestStatus.OnInspection,
        };

        var visitor = new Visitor();
        request.Visitors.Add(visitor);

        RequestVM = new RequestVM(request);
        VisitorVM = new VisitorVM(visitor) { CanEditPhoto = true };
        VisitorVM.ResetData();
    }

    #endregion
}