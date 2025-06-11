using System.Threading.Tasks;
using Proto.Command.TimerSettings;

namespace Client.Avalonia.Communication.Commands;

public interface ITimerSettingsCommandSender
{
    Task<bool> Send(CreateTimerSettingsCommand createTicketCommand);
    Task<bool> Send(ChangeSnapshotSaveIntervalCommand command);
    Task<bool> Send(ChangeDocuTimerSaveIntervalCommand command);
}