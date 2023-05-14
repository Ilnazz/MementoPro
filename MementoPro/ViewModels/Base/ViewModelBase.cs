using CommunityToolkit.Mvvm.ComponentModel;

namespace MementoPro.ViewModels.Base;

public partial class ViewModelBase : ObservableValidator
{
    [ObservableProperty]
    private string _title = null!;
}
