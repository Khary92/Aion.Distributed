using Client.Desktop.Services.LocalSettings.Commands;

namespace Client.Desktop.Services.LocalSettings;

public interface ILocalSettingsCommandSender
{
    void Send(SetExportPathCommand command);
    void Send(SetWorkDaySelectionCommand command);
}