using Domain.Events.TimerSettings;
using Service.Server.Communication.CQRS.Commands.Entities.TimerSettings;

namespace Service.Server.Translators.TimerSettings;

public interface ITimerSettingsCommandsToEventTranslator
{
    TimerSettingsEvent ToEvent(ChangeDocuTimerSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(ChangeSnapshotSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(CreateTimerSettingsCommand command);
}