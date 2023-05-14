using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public sealed partial class MainVM : WindowViewModelBase
{
    #region [ Properties ]

    public static MainVM Instance { get; private set; } = null!;

    public UserMenuVM UserMenuVM { get; set; }

    [ObservableProperty]
    private ViewModelBase _currentVM;

    #endregion

    #region [ Commands ]

    [RelayCommand]
    public void Navigate(Type viewModelType)
    {
        CurrentVM = (ViewModelBase)Activator.CreateInstance(viewModelType)!;

        GoBackCommand.NotifyCanExecuteChanged();
    }



    [RelayCommand(CanExecute = nameof(CanGoBack))]
    public void GoBack()
    {
        //if (MessageBox.Show("Несохранённые данные будут потеряны. Продолжить?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.No)
        //    return;

        CurrentVM = new ChooseRegFormVM();

        GoBackCommand.NotifyCanExecuteChanged();
    }
    private bool CanGoBack() => CurrentVM is not ChooseRegFormVM;

    #endregion

    #region [ Setup ]

    public MainVM()
    {
        Title = "ХранительПро";

        Instance = this;
        
        UserMenuVM = new(this);

        if (App.CurrentUser!.IsEmployee())
            CurrentVM = new RequestListVM();
        else
            CurrentVM = new ChooseRegFormVM();
    }

    #endregion
}
