using Core.Server.Communication.Endpoints.TimerSettings;
using Core.Server.Communication.Records.Commands.Entities.TimerSettings;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.TimerSettings;
using Domain.Events.TimerSettings;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimerSettings;

public class TimerSettingsCommandsService(
    TimerSettingsNotificationService notificationService,
    IEventStore<TimerSettingsEvent> timerSettingsEventStore,
    ITimerSettingsCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer) : ITimerSettingsCommandsService
{
    public async Task ChangeSnapshotInterval(ChangeSnapshotSaveIntervalCommand command)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.TimerSettings.ChangeSnapshotInterval.EventPersisted(GetType(), command.TraceId,
            notification.SnapshotSaveIntervalChanged);

        await tracer.TimerSettings.ChangeSnapshotInterval.SendingNotification(GetType(), command.TraceId,
            notification.SnapshotSaveIntervalChanged);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand command)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.TimerSettings.ChangeDocuTimerInterval.EventPersisted(GetType(), command.TraceId,
            notification.DocuTimerSaveIntervalChanged);

        await tracer.TimerSettings.ChangeDocuTimerInterval.SendingNotification(GetType(), command.TraceId,
            notification.DocuTimerSaveIntervalChanged);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }
}