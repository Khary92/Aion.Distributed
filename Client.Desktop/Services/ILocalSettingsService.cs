using System;
using System.Threading.Tasks;
using Client.Desktop.DTO;

namespace Client.Desktop.Services;

public interface ILocalSettingsService
{
    SettingsDto LocalSettings { get; }
    Task SetExportPath(string exportPath);
    string ExportPath { get; }
    bool IsExportPathValid();
    DateTimeOffset SelectedDate { get; set; }
    bool IsSelectedDateCurrentDate();
}