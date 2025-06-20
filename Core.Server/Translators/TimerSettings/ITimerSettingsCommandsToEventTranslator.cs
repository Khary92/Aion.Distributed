using Core.Server.Communication.CQRS.Commands.Entities.TimerSettings;
using Domain.Events.TimerSettings;

namespace Core.Server.Translators.TimerSettings;

public interface ITimerSettingsCommandsToEventTranslator
{
    TimerSettingsEvent ToEvent(ChangeDocuTimerSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(ChangeSnapshotSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(CreateTimerSettingsCommand command);
}