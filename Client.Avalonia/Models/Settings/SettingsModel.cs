using System.Threading.Tasks;
using Contract.DTO;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Settings;

public class SettingsModel(IMediator mediator) : ReactiveObject
{
    private SettingsDto _settingsDto = null!;

    public SettingsDto Settings
    {
        get => _settingsDto;
        private set => this.RaiseAndSetIfChanged(ref _settingsDto, value);
    }

    public async Task InitializeAsync()
    {
        Settings = (await mediator.Send(new GetSettingsRequest()))!;
    }

    public async Task SaveConfigAsync()
    {
        //TODO Make better Events and implement property changed
        var updateSettingsCommand = new UpdateSettingsCommand(Settings.SettingsId, Settings.ExportPath,
            Settings.IsAddNewTicketsToCurrentSprintActive);
        await mediator.Send(updateSettingsCommand);

        //  logger.LogCommandSentNoAnswer(updateSettingsCommand);
    }
}