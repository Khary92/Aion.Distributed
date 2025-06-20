using Service.Server.CQRS.Commands.Entities.TimerSettings;

namespace Service.Server.Old.Services.Entities.TimerSettings;

public interface ITimerSettingsCommandsService
{
    Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand changeSnapshotSaveIntervalCommand);
    Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand changeDocuTimerSaveIntervalCommand);
    Task Create(CreateTimerSettingsCommand createSettingsCommand);
}