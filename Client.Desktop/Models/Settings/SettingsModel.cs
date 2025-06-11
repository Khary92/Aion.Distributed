using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Contract.DTO;
using Proto.Command.Settings;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class SettingsModel(ICommandSender commandSender, IRequestSender requestSender) : ReactiveObject
{
    private SettingsDto _settingsDto = null!;

    public SettingsDto Settings
    {
        get => _settingsDto;
        private set => this.RaiseAndSetIfChanged(ref _settingsDto, value);
    }

    public async Task InitializeAsync()
    {
        Settings = await requestSender.GetSettings();
    }

    public async Task SaveConfigAsync()
    {
        //TODO Make better Events and implement property changed
        var updateSettingsCommand = new UpdateSettingsCommand
        {
            SettingsId = Settings.SettingsId.ToString(),
            ExportPath = Settings.ExportPath,
            IsAddNewTicketsToCurrentSprintActive = Settings.IsAddNewTicketsToCurrentSprintActive,
        };
        await commandSender.Send(updateSettingsCommand);
    }
}