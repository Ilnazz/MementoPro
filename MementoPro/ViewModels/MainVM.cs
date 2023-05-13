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
    private ViewModelBase _currentVM;

    [ObservableProperty]
    private string _currentViewTitle;

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void GoToPersonalRegForm()
    {
        CurrentVM = new PersonalRegFormVM(this);
        GoBackCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void GoToGroupRegForm()
    {
        //CurrentVM = new PersonalRegFormVM();
        GoBackCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    public void GoBack()
    {
        if (CurrentVM.RequestClose() == false)
            return;

        CurrentVM = new ChooseRegFormVM(this);
        GoBackCommand.NotifyCanExecuteChanged();
    }
    private bool CanGoBack() => CurrentVM is not ChooseRegFormVM;

    #endregion

    #region [ Setup ]

    public MainVM()
    {
        Title = "ХранительПро";

        UserMenuVM = new(this);

        if (App.CurrentUser.IsEmployee())
            CurrentVM = new RequestListVM(this);
        else
            CurrentVM = new ChooseRegFormVM(this);
    }

    #endregion
}
