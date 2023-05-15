using System.Linq;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public partial class AddEditRequestRejectionReasonVM : WindowViewModelBase
{
    #region [ Properties ]

    public RequestRejectionReason RejectionReason { get; init; }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void Save()
    {
        CloseWindow();
    }

    #endregion

    #region [ Fields ]

    private Request _request = null!;

    #endregion

    public AddEditRequestRejectionReasonVM(Request requeset)
    {
        _request = requeset;

        if (requeset.RequestRejectionReasons.Count == 0)
        {
            var newRequestRejectionReason = new RequestRejectionReason();
            App.Db.RequestRejectionReasons.Local.Add(newRequestRejectionReason);
            requeset.RequestRejectionReasons.Add(newRequestRejectionReason);
        }

        RejectionReason = _request.RequestRejectionReasons.First();
    }
}