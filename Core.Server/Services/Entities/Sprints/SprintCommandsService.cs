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
        var sprintNotification = command.ToNotification();
        await tracer.Sprint.Create.EventPersisted(GetType(), command.TraceId, sprintNotification.SprintCreated);

        await tracer.Sprint.Create.SendingNotification(GetType(), command.TraceId, sprintNotification.SprintCreated);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task UpdateSprintData(UpdateSprintDataCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        var sprintNotification = command.ToNotification();
        await tracer.Sprint.Update.EventPersisted(GetType(), command.TraceId, sprintNotification.SprintDataUpdated);

        await tracer.Sprint.Update.SendingNotification(GetType(), command.TraceId,
            sprintNotification.SprintDataUpdated);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task SetSprintActiveStatus(SetSprintActiveStatusCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        var sprintNotification = command.ToNotification();
        await tracer.Sprint.ActiveStatus.EventPersisted(GetType(), command.TraceId,
            sprintNotification.SprintActiveStatusSet);

        await tracer.Sprint.ActiveStatus.SendingNotification(GetType(), command.TraceId,
            sprintNotification.SprintActiveStatusSet);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task AddTicketToSprint(AddTicketToSprintCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command));
        var sprintNotification = command.ToNotification();
        await tracer.Sprint.AddTicketToSprint.EventPersisted(GetType(), command.TraceId,
            sprintNotification.TicketAddedToActiveSprint);

        await tracer.Sprint.AddTicketToSprint.SendingNotification(GetType(), command.TraceId,
            sprintNotification.TicketAddedToActiveSprint);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }
}