using Core.Server.Communication.Endpoints.TimerSettings;
using Core.Server.Communication.Records.Commands.Entities.TimerSettings;
using Core.Server.Translators.Commands.TimerSettings;
using Domain.Events.TimerSettings;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimerSettings;

public class TimerSettingsCommandsService(
    TimerSettingsNotificationService notificationService,
    IEventStore<TimerSettingsEvent> timerSettingsEventStore,
    ITimerSettingsCommandsToEventTranslator eventTranslator) : ITimerSettingsCommandsService
{
    public async Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand command)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand command)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateTimerSettingsCommand command)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }
}