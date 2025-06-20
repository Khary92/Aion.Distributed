using Service.Server.Communication.CQRS.Commands.Entities.TimerSettings;

namespace Service.Server.Services.Entities.TimerSettings;

public interface ITimerSettingsCommandsService
{
    Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand changeSnapshotSaveIntervalCommand);
    Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand changeDocuTimerSaveIntervalCommand);
    Task Create(CreateTimerSettingsCommand createSettingsCommand);
}