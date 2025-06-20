using Core.Server.Communication.CQRS.Commands.Entities.TimerSettings;
using Core.Server.Communication.Services.TimerSettings;
using Core.Server.Translators.TimerSettings;
using Domain.Events.TimerSettings;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimerSettings;

public class TimerSettingsCommandsService(
    TimerSettingsNotificationService notificationService,
    IEventStore<TimerSettingsEvent> timerSettingsEventStore,
    ITimerSettingsCommandsToEventTranslator eventTranslator) : ITimerSettingsCommandsService
{
    public async Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand changeSnapshotSaveIntervalCommand)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(changeSnapshotSaveIntervalCommand));
        await notificationService.SendNotificationAsync(changeSnapshotSaveIntervalCommand.ToNotification());
    }

    public async Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand changeDocuTimerSaveIntervalCommand)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(changeDocuTimerSaveIntervalCommand));
        await notificationService.SendNotificationAsync(changeDocuTimerSaveIntervalCommand.ToNotification());
    }

    public async Task Create(CreateTimerSettingsCommand createSettingsCommand)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(createSettingsCommand));
        await notificationService.SendNotificationAsync(createSettingsCommand.ToNotification());
    }
}