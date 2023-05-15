using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using ExcelDataReader;
using MementoPro.Database.Models;
using MementoPro.ViewModels.Base;
using Microsoft.Win32;

namespace MementoPro.ViewModels;

public partial class VisitorListVM : ViewModelBase
{
    #region [ Properties ]

    public ObservableCollection<VisitorVM> VisitorViewModels { get; } = new();

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void LoadVisitorList()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel files (*.xlsx)|*.xlsx",
            CheckFileExists = true
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        var filePath = openFileDialog.FileName;

        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        // Init reader
        reader.Read();
        // Skip column headers row
        reader.Read();

        int fullNameCol = 0,
            phoneCol = 1,
            emailCol = 2,
            birthDateCol = 3,
            passportDataCol = 4;

        var visitors = new List<Visitor>();

        while (true)
        {
            // End of data stream
            if (string.IsNullOrWhiteSpace(reader.GetString(0)))
                break;

            var nameParts = reader.GetString(fullNameCol).Split(' ');
            var passportParts = reader.GetString(passportDataCol).Split(' ');

            visitors.Add(new Visitor
            {
                LastName = nameParts[0],
                FirstName = nameParts[1],
                Patronymic = nameParts[2],
                PassportSeries = passportParts[0],
                PassportNumber = passportParts[1],
                Email = reader.GetString(emailCol),
                Phone = reader.GetString(phoneCol),
                BirthDate = GetBirthDate(reader.GetString(birthDateCol)),
            });

            reader.Read();
        }

        visitors.Select(v => new VisitorVM(v)).ToList().ForEach(VisitorViewModels.Add);
    }

    private readonly CultureInfo _cultureInfoRussia = new("ru-RU");
    private DateTime GetBirthDate(string birthDateStr)
    {
        var transformedBirthDateStr = Regex.Replace(birthDateStr, " года", "");
        return DateTime.Parse(transformedBirthDateStr, _cultureInfoRussia);
    }


    [RelayCommand]
    private async void DownloadVisitorListTemplate()
    {
        var dlg = new SaveFileDialog
        {
            FileName = "Шаблон списка посетителей",
            DefaultExt = ".xlsx",
            Filter = "Excel files (*.xlsx)|*.xlsx"
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

    public VisitorListVM()
    {

    }

    #endregion
}
