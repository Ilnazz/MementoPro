using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MementoPro.ViewModels.Base;

public partial class ViewModelBase : ObservableValidator
{
    [ObservableProperty]
    private string _title = null!;

    public bool RequestClose()
        => MessageBox.Show(
                "Несохранённые данные будут потеряны. Продолжить?",
                "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
}
