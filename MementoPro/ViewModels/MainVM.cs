using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.ViewModels.Base;
using System.Windows;

namespace MementoPro.ViewModels;

public sealed partial class MainVM : WindowViewModelBase
{
    #region [ Properties ]

    public UserMenuVM UserMenuVM { get; set; }

    [ObservableProperty]
    private ObservableObject? _currentVM;

    [ObservableProperty]
    private Visibility _navigationButtonsVisibility;

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void OpenPersonalRegForm()
    {
        NavigationButtonsVisibility = Visibility.Collapsed;
        CurrentVM = new PersonalRegFormVM();
    }

    [RelayCommand]
    private void OpenGroupRegForm()
    {
        //CurrentVM = new RegFormVM();
    }

    #endregion

    #region [ Setup ]

    public MainVM()
    {
        UserMenuVM = new(this);
    }

    #endregion
}
