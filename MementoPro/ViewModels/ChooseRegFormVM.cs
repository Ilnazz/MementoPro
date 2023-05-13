using MementoPro.ViewModels.Base;

namespace MementoPro.ViewModels;

public class ChooseRegFormVM : ViewModelBase
{
    public MainVM MainVM { get; init; }

    public ChooseRegFormVM(MainVM mainVM)
    {
        Title = "Выбор формы записи";

        MainVM = mainVM;
    }
}
