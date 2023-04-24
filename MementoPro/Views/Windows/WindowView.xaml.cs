using MementoPro.ViewModels.Base;
using System.ComponentModel;
using System.Windows;

namespace MementoPro.Views.Windows;

public sealed partial class WindowView : Window
{
    private readonly WindowViewModelBase _viewModel = null!;

    public WindowView(WindowViewModelBase viewModel)
    {
        DataContext = _viewModel = viewModel;

        _viewModel.CloseWindowMethod += Close;

        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
        => e.Cancel = _viewModel.OnClosing() == false;
}
