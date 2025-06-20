using Domain.Events.Sprints;
using Domain.Interfaces;
using Service.Server.Communication.Sprint;
using Service.Server.CQRS.Commands.Entities.Sprints;
using Service.Server.Old.Translators.Sprints;

namespace Service.Server.Old.Services.Entities.Sprints;

public class SprintCommandsService(
    IEventStore<SprintEvent> sprintEventStore,
    SprintNotificationService sprintsNotificationService,
    ISprintCommandsToEventTranslator eventTranslator)
    : ISprintCommandsService
{
    public async Task Create(CreateSprintCommand createSprintCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(createSprintCommand));
        await sprintsNotificationService.SendNotificationAsync(createSprintCommand.ToNotification());
    }

    public async Task UpdateSprintData(UpdateSprintDataCommand updateSprintDataCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(updateSprintDataCommand));
        await sprintsNotificationService.SendNotificationAsync(updateSprintDataCommand.ToNotification());
    }

    public async Task SetSprintActiveStatus(SetSprintActiveStatusCommand setSprintActiveStatusCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(setSprintActiveStatusCommand));
        await sprintsNotificationService.SendNotificationAsync(setSprintActiveStatusCommand.ToNotification());
    }

    public async Task AddTicketToSprint(AddTicketToSprintCommand addTicketToSprintCommand)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(addTicketToSprintCommand));
        await sprintsNotificationService.SendNotificationAsync(addTicketToSprintCommand.ToNotification());
    }
}