using System;

namespace Client.Desktop.Services.LocalSettings;

public interface ILocalSettingsService
{
    bool IsExportPathValid();
    DateTimeOffset SelectedDate { get; }
    bool IsSelectedDateCurrentDate();
}