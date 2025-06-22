using Core.Server.Communication.Records.Commands.Entities.TimerSettings;
using Domain.Events.TimerSettings;

namespace Core.Server.Translators.Commands.TimerSettings;

public interface ITimerSettingsCommandsToEventTranslator
{
    TimerSettingsEvent ToEvent(ChangeDocuTimerSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(ChangeSnapshotSaveIntervalCommand command);
    TimerSettingsEvent ToEvent(CreateTimerSettingsCommand command);
}