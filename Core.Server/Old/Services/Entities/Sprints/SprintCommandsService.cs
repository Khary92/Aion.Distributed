using Domain.Events.Sprints;
using Domain.Interfaces;
using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.Old.Translators.Sprints;

namespace Service.Server.Old.Services.Entities.Sprints;

public class SprintCommandsService(
    IEventStore<SprintEvent> sprintEventStore,
    IMediator mediator,
    ISprintCommandsToEventTranslator eventTranslator,
    ISprintCommandsToNotificationTranslator notificationTranslator)
    : ISprintCommandsService
{
    public async Task Create(CreateSprintCommand createSprintCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(createSprintCommand));
        await mediator.Publish(notificationTranslator.ToNotification(createSprintCommand));
    }

    public async Task UpdateSprintData(UpdateSprintDataCommand updateSprintDataCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(updateSprintDataCommand));
        await mediator.Publish(notificationTranslator.ToNotification(updateSprintDataCommand));
    }

    public async Task SetSprintActiveStatus(SetSprintActiveStatusCommand setSprintActiveStatusCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(setSprintActiveStatusCommand));
        await mediator.Publish(notificationTranslator.ToNotification(setSprintActiveStatusCommand));
    }

    public async Task AddTicketToSprint(AddTicketToSprintCommand addTicketToSprintCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(addTicketToSprintCommand));
        await mediator.Publish(notificationTranslator.ToNotification(addTicketToSprintCommand));
    }
}