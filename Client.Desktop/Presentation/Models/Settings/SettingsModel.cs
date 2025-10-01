using System;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.LocalSettings;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Settings;

public class SettingsModel(ILocalSettingsService localSettingsService) : ReactiveObject, IInitializeAsync
{
    private string? _exportPath;
    private DateTimeOffset _selectedDate;

    public string? ExportPath
    {
        get => _exportPath;
        set => this.RaiseAndSetIfChanged(ref _exportPath, value);
    }

    public DateTimeOffset SelectedDate
    {
        get => _selectedDate;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDate, value);
            //TODO This is probably wrong. Needs fixing sometime
            localSettingsService.SetSelectedDate(value);
        }
    }

    public async Task SetExportPath()
    {
        if (_exportPath == null) return;
        
        await localSettingsService.SetExportPath(_exportPath);
    }

    public InitializationType Type => InitializationType.Model;

    public Task InitializeAsync()
    {
        ExportPath = localSettingsService.ExportPath;
        SelectedDate = localSettingsService.SelectedDate;
        return Task.CompletedTask;
    }
}