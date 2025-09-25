using Core.Server.Communication.Records.Commands.Entities.TimerSettings;

namespace Core.Server.Services.Entities.TimerSettings;

public interface ITimerSettingsCommandsService
{
    Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand command);
    Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand command);
}