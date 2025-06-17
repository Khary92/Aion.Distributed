using Application.Contract.CQRS.Commands.Entities.TimerSettings;

namespace Application.Services.Entities.TimerSettings;

public interface ITimerSettingsCommandsService
{
    Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand changeSnapshotSaveIntervalCommand);
    Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand changeDocuTimerSaveIntervalCommand);
    Task Create(CreateTimerSettingsCommand createSettingsCommand);
}