using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;
using MementoPro.Views.Windows;

namespace MementoPro.ViewModels;

public partial class RequestListVM : ViewModelBase
{
    #region [ Properties ]

    public IEnumerable<Request> Requests { get; }

    public IEnumerable<RequestStatus> RequestStatuses { get; } = App.Db.RequestStatuses.Local.ToList();

    public bool IsGeneralDepartmentEmployee => App.CurrentUser!.Employee!.IsGeneralDepartmentEmployee;

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void AddEditRequestRejectionReason(Request request)
    {
        new WindowView(new AddEditRequestRejectionReasonVM(request)).ShowDialog();
    }

    [RelayCommand]
    private void SaveChanges()
    {
        App.Db.SaveChanges();
        MessageBox.Show("Изменения сохранены");
    }

    #endregion

    public RequestListVM()
    {
        Title = "Заявки на посещение мероприятия";

        var employee = App.CurrentUser!.Employee!;
        var isGeneralDepartmentEmployee = employee.IsGeneralDepartmentEmployee;
        Requests = isGeneralDepartmentEmployee
            ? App.Db.Requests.Local.ToList()
            : App.Db.Requests.Local.Where(r => r.Employee == employee && r.RequestStatusId == (int)DataTypes.Enums.RequestStatus.Approved).ToList();
    }
}
