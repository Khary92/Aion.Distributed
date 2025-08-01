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
    public async Task Create(CreateSprintCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task UpdateSprintData(UpdateSprintDataCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task SetSprintActiveStatus(SetSprintActiveStatusCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task AddTicketToSprint(AddTicketToSprintCommand command)
    {
        await sprintEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await sprintsNotificationService.SendNotificationAsync(command.ToNotification());
    }
}