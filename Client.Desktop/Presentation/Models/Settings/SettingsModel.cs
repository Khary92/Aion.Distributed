using System;
using System.Threading.Tasks;
using Client.Desktop.Services.LocalSettings;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Settings;

public class SettingsModel : ReactiveObject
{
    private readonly ILocalSettingsService _localSettingsService;

    private string _exportPath;
    private DateTimeOffset _selectedDate;

    public SettingsModel(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;

        _exportPath = _localSettingsService.ExportPath;
        _selectedDate = _localSettingsService.SelectedDate;
    }

    public string ExportPath
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
            _localSettingsService.SetSelectedDate(value);
        }
    }
    
    public async Task SetExportPath()
    {
       await _localSettingsService.SetExportPath(_exportPath);
    }
}