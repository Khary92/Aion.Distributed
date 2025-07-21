using Client.Desktop.Services.LocalSettings.Commands;
using CommunityToolkit.Mvvm.Messaging;

namespace Client.Desktop.Services.LocalSettings;

public class LocalSettingsCommandSender(IMessenger messenger) : ILocalSettingsCommandSender
{
    public void Send(SetExportPathCommand command)
    {
        messenger.Send(command);
    }
}