using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.ViewModels.Base;
using MementoPro.Views;
using MementoPro.Views.Windows;
using Models;
using System.Windows;

namespace MementoPro.ViewModels;

public partial class UserMenuVM : ObservableObject
{
    public string Name => Session.CurrentUser!.Name;

    [RelayCommand]
    private void LogOut()
    {
        var answer = MessageBox.Show("Выйти из системы?", "Вопрос",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer == MessageBoxResult.Yes)
        {
            Session.CurrentUser = null;

            new WindowView(new AuthVM()).Show();
            _parent.CloseWindow();
        }
    }
    
    private WindowViewModelBase _parent = null!;
    public UserMenuVM(WindowViewModelBase parent)
        => _parent = parent;
}
