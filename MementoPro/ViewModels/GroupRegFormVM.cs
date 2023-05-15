using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public partial class GroupRegFormVM : ViewModelBase
{
    #region [ Properties ]

    [ObservableProperty]
    private VisitorVM _visitorVM;

    public VisitorListVM VisitorListVM { get; init; }

    public RequestVM RequestVM { get; init; }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void AddVisitor()
    {
        if (VisitorVM.Validate() == false)
            return;

        VisitorListVM.VisitorViewModels.Add(VisitorVM);
        VisitorVM = new VisitorVM(new());
        VisitorVM.ResetData();
    }



    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private void Submit()
    {
        if (RequestVM.Validate() == false)
            return;

        if (VisitorListVM.VisitorViewModels.Count < 2)
        {
            MessageBox.Show("Необходимо предоставить данные как минимум для двух посетителей", "Ошибка");
            return;
        }

        VisitorListVM.VisitorViewModels
            .Select(vvm => vvm.Visitor)
            .ToList()
            .ForEach((v) =>
            {
                App.Db.Visitors.Add(v);
                RequestVM.Request.Visitors.Add(v);
            });
        App.Db.Requests.Add(RequestVM.Request);
        App.Db.SaveChanges();

        MessageBox.Show("Заявка зарегистрирована", "Сообщение");

        MainVM.Instance.GoBack();
    }
    private bool CanSubmit() => HasErrors == false;

    [RelayCommand]
    private void ResetData()
    {
        RequestVM.ResetData();
        VisitorVM.ResetData();
        VisitorListVM.VisitorViewModels.Clear();
    }

    #endregion

    #region [ Setup ]

    public GroupRegFormVM()
    {
        Title = "Групповая форма регистрации на посещение мероприятия";

        var request = new Request
        {
            User = App.CurrentUser,
            RequestTypeId = (int)DataTypes.Enums.RequestType.Group,
            RequestStatusId = (int)DataTypes.Enums.RequestStatus.OnInspection,
        };

        RequestVM = new(request);
        VisitorVM = new(new());
        VisitorVM.ResetData();
        VisitorListVM = new();
    }

    #endregion
}
