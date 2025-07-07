using System.Threading.Tasks;
using Proto.Command.TimerSettings;

namespace Client.Desktop.Communication.Commands.TimerSettings;

public interface ITimerSettingsCommandSender
{
    Task<bool> Send(CreateTimerSettingsCommandProto command);
    Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command);
    Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command);
}