using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.ViewModels.Base;
using MementoPro.Views;
using MementoPro.Views.Windows;
using System.Windows;

namespace MementoPro.ViewModels;

public partial class UserMenuVM : ViewModelBase
{
    public string Name => App.CurrentUser?.Name ?? "Гость";

    [RelayCommand]
    private void DoLogin()
    {
        if (App.Db.ChangeTracker.HasChanges()
            && MessageBox.Show("Несохранённые данные будут потеряны. Продолжить?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.No)
        {
            App.Db.RollBack();
            return;
        }

        App.CurrentUser = null;

        new WindowView(new AuthVM()).Show();
        _parent.CloseWindow();
    }

    private WindowViewModelBase _parent = null!;
    public UserMenuVM(WindowViewModelBase parent)
        => _parent = parent;
}
