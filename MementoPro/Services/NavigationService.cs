using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MementoPro.Services;

public static class NavigationService
{
    private static readonly Stack<object> _history = new();

    private static ContentControl _viewContentControl = null!;

    public static void Setup(ContentControl viewContentControl)
    {
        _viewContentControl = viewContentControl;
    }

    public static void Navigate(Type viewModelType)
    {
        var viewModel = Activator.CreateInstance(viewModelType)!;

        _history.Push(viewModel);
        _viewContentControl.Content = viewModel;
    }

    public static void GoBack()
    {
        if (_history.Count == 0)
            return;

        _history.Pop();
        _viewContentControl.Content = _history.Peek();
    }
}
