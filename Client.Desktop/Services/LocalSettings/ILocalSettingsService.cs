using System;
using System.Threading.Tasks;

namespace Client.Desktop.Services.LocalSettings;

public interface ILocalSettingsService
{
    DateTimeOffset SelectedDate { get; }
    bool IsExportPathValid();
    bool IsSelectedDateCurrentDate();
    string ExportPath { get; }
    Task SetSelectedDate(DateTimeOffset date);
    Task SetExportPath(string path);
    
}