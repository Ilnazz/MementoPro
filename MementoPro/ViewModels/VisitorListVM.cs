using System;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using MementoPro.ViewModels.Base;
using Microsoft.Win32;

namespace MementoPro.ViewModels;

public partial class VisitorListVM : ViewModelBase
{
    #region [ Properties ]

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void LoadVisitorList()
    {

    }

    [RelayCommand]
    private async void DownloadVisitorListTemplate()
    {
        var dlg = new SaveFileDialog
        {
            FileName = "Шаблон списка посетителей",
            DefaultExt = ".xlsx",
            Filter = "Excel (*.xlsx)|*.xlsx"
        };

        var result = dlg.ShowDialog();
        if (result == true)
        {
            var filePathToSave = dlg.FileName;

            var visitorListTemplateFileUri = new Uri("Resources/visitor_list_template.xlsx", UriKind.Relative);
            var visitorListTemplateFileStream = Application.GetResourceStream(visitorListTemplateFileUri).Stream;
            await visitorListTemplateFileStream.CopyToAsync(new FileStream(filePathToSave, FileMode.OpenOrCreate));
        }
    }

    #endregion

    #region [ Setup ]

    #endregion
}
