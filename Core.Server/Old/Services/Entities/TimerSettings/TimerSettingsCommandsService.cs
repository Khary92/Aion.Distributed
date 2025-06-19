using Domain.Events.TimerSettings;
using Domain.Interfaces;
using Service.Server.CQRS.Commands.Entities.TimerSettings;
using Service.Server.Old.Translators.TimerSettings;

namespace Service.Server.Old.Services.Entities.TimerSettings;

public class TimerSettingsCommandsService(
    IMediator mediator,
    IEventStore<TimerSettingsEvent> timerSettingsEventStore,
    ITimerSettingsCommandsToEventTranslator eventTranslator,
    ITimerSettingsCommandsToNotificationTranslator notificationTranslator) : ITimerSettingsCommandsService
{
    public async Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand changeSnapshotSaveIntervalCommand)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(changeSnapshotSaveIntervalCommand));
        await mediator.Publish(notificationTranslator.ToNotification(changeSnapshotSaveIntervalCommand));
    }

    public async Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand changeDocuTimerSaveIntervalCommand)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(changeDocuTimerSaveIntervalCommand));
        await mediator.Publish(notificationTranslator.ToNotification(changeDocuTimerSaveIntervalCommand));
    }

    public async Task Create(CreateTimerSettingsCommand createSettingsCommand)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(createSettingsCommand));
        await mediator.Publish(notificationTranslator.ToNotification(createSettingsCommand));
    }
}