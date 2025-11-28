using Proto.Command.TimerSettings;

namespace Service.Admin.Web.Communication.Commands.TimerSettings;

public interface ITimerSettingsCommandSender
{
    Task<bool> Send(CreateTimerSettingsCommandProto command);
    Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command);
    Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command);
}