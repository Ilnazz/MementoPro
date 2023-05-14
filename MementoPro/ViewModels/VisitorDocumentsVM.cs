using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;
using Microsoft.Win32;

namespace MementoPro.ViewModels;

public partial class VisitorDocumentsVM : ViewModelBase
{
    #region [ Properties ]

    public string PassportScanFileName
    {
        get => _visitor.PassportScanFileName;
        set
        {
            _visitor.PassportScanFileName = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region [ Commands ]

    [RelayCommand(CanExecute = nameof(CanAttachPassportScan))]
    private void AttachPassportScan()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "PDF files|*.pdf",
            CheckFileExists = true
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        //Get the path of specified file
        var filePath = openFileDialog.FileName;
        var file = File.ReadAllBytes(filePath);

        PassportScanFileName = Path.GetFileName(openFileDialog.FileName);
        _visitor.PassportScan = file;

        AttachPassportScanCommand.NotifyCanExecuteChanged();
    }
    private bool CanAttachPassportScan() => _visitor.PassportScan == null;



    [RelayCommand]
    private void ClearPassportScan()
    {
        PassportScanFileName = string.Empty;
        _visitor.PassportScan = null!;

        AttachPassportScanCommand.NotifyCanExecuteChanged();
    }

    #endregion

    #region [ Public methods ]

    public void ResetData() => ClearPassportScan();

    public bool Validate()
    {
        if (_visitor.PassportScan == null)
        {
            MessageBox.Show("Необходимо прикрепить скан паспорта");
            return false;
        }

        return true;
    }

    #endregion

    #region [ Fields ]

    private readonly Visitor _visitor = null!;

    #endregion

    #region [ Setup ]

    public VisitorDocumentsVM(Visitor visitor)
    {
        _visitor = visitor;

        ResetData();
    }

    #endregion
}
