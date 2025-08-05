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
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var ticketNotification = command.ToNotification();
        await tracer.TimerSettings.ChangeSnapshotInterval.EventPersisted(GetType(), command.TraceId,
            ticketNotification.SnapshotSaveIntervalChanged);

        await tracer.TimerSettings.ChangeSnapshotInterval.SendingNotification(GetType(), command.TraceId,
            ticketNotification.SnapshotSaveIntervalChanged);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeDocumentationInterval(ChangeDocuTimerSaveIntervalCommand command)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var ticketNotification = command.ToNotification();
        await tracer.TimerSettings.Create.EventPersisted(GetType(), command.TraceId,
            ticketNotification.DocuTimerSaveIntervalChanged);

        await tracer.TimerSettings.Create.SendingNotification(GetType(), command.TraceId,
            ticketNotification.DocuTimerSaveIntervalChanged);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateTimerSettingsCommand command)
    {
        await timerSettingsEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var ticketNotification = command.ToNotification();
        await tracer.TimerSettings.Create.EventPersisted(GetType(), command.TraceId,
            ticketNotification.TimerSettingsCreated);

        await tracer.TimerSettings.Create.SendingNotification(GetType(), command.TraceId,
            ticketNotification.TimerSettingsCreated);
        await notificationService.SendNotificationAsync(command.ToNotification());
    }
}