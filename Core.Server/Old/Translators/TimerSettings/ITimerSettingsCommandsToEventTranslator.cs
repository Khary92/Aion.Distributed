using Application.Contract.CQRS.Commands.Entities.TimerSettings;
using Domain.Events.TimerSettings;

namespace Application.Translators.TimerSettings;

public interface ITimerSettingsCommandsToEventTranslator
{
    TimerSettingsEvent ToEvent(ChangeDocuTimerSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(ChangeSnapshotSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(CreateTimerSettingsCommand command);
}