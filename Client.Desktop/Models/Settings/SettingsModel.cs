using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.Services;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class SettingsModel(ILocalSettingsService localSettingsService)
    : ReactiveObject
{
    private SettingsDto _settingsDto = localSettingsService.LocalSettings;

    public SettingsDto Settings
    {
        get => _settingsDto;
        private set => this.RaiseAndSetIfChanged(ref _settingsDto, value);
    }

    public async Task SetExportPath(string exportPath)
    {
        await localSettingsService.SetExportPath(exportPath);
    }
}