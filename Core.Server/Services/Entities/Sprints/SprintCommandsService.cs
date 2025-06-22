using Core.Server.Communication.Endpoints.Sprint;
using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Core.Server.Translators.Commands.Sprints;
using Domain.Events.Sprints;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Sprints;

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