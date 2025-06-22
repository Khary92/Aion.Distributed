using Core.Server.Communication.Records.Commands.Entities.TimerSettings;

namespace Core.Server.Services.Entities.TimerSettings;

public interface ITimerSettingsCommandsService
{
    Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand changeSnapshotSaveIntervalCommand);
    Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand changeDocuTimerSaveIntervalCommand);
    Task Create(CreateTimerSettingsCommand createSettingsCommand);
}