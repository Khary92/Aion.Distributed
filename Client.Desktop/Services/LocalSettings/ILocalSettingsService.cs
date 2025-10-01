using System;
using System.Threading.Tasks;

namespace Client.Desktop.Services.LocalSettings;

public interface ILocalSettingsService
{
    DateTimeOffset SelectedDate { get; }
    string ExportPath { get; }
    bool IsExportPathValid();
    bool IsSelectedDateCurrentDate();
    Task SetSelectedDate(DateTimeOffset date);
    Task SetExportPath(string path);
}