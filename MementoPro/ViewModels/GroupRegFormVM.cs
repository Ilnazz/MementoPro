using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public partial class GroupRegFormVM : ViewModelBase
{
    #region [ Properties ]

    public VisitorVM VisitorVM { get; init; }

    public VisitorListVM VisitorListVM { get; init; }

    public RequestVM RequestVM { get; init; }

    #endregion

    #region [ Commands ]

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private void Submit()
    {
        //if (VisitorVM.Validate() == false || RequestVM.Validate() == false)
        //    return;

        //App.Db.Requests.Add(RequestVM.Request);
        //App.Db.Visitors.Add(VisitorVM.Visitor);

        //App.Db.SaveChanges();

        MainVM.Instance.GoBack();
    }
    private bool CanSubmit() => HasErrors == false;

    [RelayCommand]
    private void ResetData()
    {
        RequestVM.ResetData();
    }

    #endregion

    #region [ Setup ]

    public GroupRegFormVM()
    {
        Title = "Групповая форма регистрации на посещение мероприятия";

        var request = new Request
        {
            RequestTypeId = (int)DataTypes.Enums.RequestType.Group,
            RequestStatusId = (int)DataTypes.Enums.RequestStatus.OnInspection,
        };

        var visitor = new Visitor();
        request.Visitors.Add(visitor);

        RequestVM = new(request);
        VisitorVM = new(visitor) { CanEditPhoto = false };
        VisitorListVM = new();
    }

    #endregion
}
