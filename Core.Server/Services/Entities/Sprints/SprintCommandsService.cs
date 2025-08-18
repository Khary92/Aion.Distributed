using Core.Server.Communication.Endpoints.Sprint;
using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Sprints;
using Domain.Events.Sprints;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Sprints;

public class SprintCommandsService(
    IEventStore<SprintEvent> sprintEventStore,
    SprintNotificationService sprintsNotificationService,
    ISprintCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer)
    : ISprintCommandsService
{
    public async Task Create(CreateSprintCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.Sprint.Create.EventPersisted(GetType(), command.TraceId, notification);

        await tracer.Sprint.Create.SendingNotification(GetType(), command.TraceId, notification);
        await sprintsNotificationService.SendNotificationAsync(notification);
    }

    public async Task UpdateSprintData(UpdateSprintDataCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.Sprint.Update.EventPersisted(GetType(), command.TraceId, notification);

        await tracer.Sprint.Update.SendingNotification(GetType(), command.TraceId, notification);
        await sprintsNotificationService.SendNotificationAsync(notification);
    }

    public async Task SetSprintActiveStatus(SetSprintActiveStatusCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.Sprint.ActiveStatus.EventPersisted(GetType(), command.TraceId, notification);

        await tracer.Sprint.ActiveStatus.SendingNotification(GetType(), command.TraceId, notification);
        await sprintsNotificationService.SendNotificationAsync(notification);
    }

    public async Task AddTicketToSprint(AddTicketToSprintCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command));

        var notification = command.ToNotification();
        await tracer.Sprint.AddTicketToSprint.EventPersisted(GetType(), command.TraceId, notification);

        await tracer.Sprint.AddTicketToSprint.SendingNotification(GetType(), command.TraceId, notification);
        await sprintsNotificationService.SendNotificationAsync(notification);
    }
}