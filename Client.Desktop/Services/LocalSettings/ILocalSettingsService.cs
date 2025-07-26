using System;

namespace Client.Desktop.Services.LocalSettings;

public interface ILocalSettingsService
{
    DateTimeOffset SelectedDate { get; }
    bool IsExportPathValid();
    bool IsSelectedDateCurrentDate();
}