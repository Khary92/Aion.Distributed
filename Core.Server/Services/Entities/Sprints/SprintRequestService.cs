using Domain.Entities;
using Domain.Events.Sprints;
using Domain.Interfaces;
using Service.Server.Communication.CQRS.Commands.Entities.Sprints;

namespace Service.Server.Services.Entities.Sprints;

public class SprintRequestService(IEventStore<SprintEvent> sprintEventsStore, SprintCommandsService sprintCommandsService) : ISprintRequestsService
{
    public async Task<List<Sprint>> GetAll()
    {
        var tickets = (await sprintEventsStore.GetAllEventsAsync())
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .Select(Sprint.Rehydrate).ToList();

        return tickets;
    }

    public async Task<Sprint?> GetById(Guid id)
    {
        var sprintEvents = await sprintEventsStore
            .GetEventsForAggregateAsync(id);

        return Sprint.Rehydrate(sprintEvents);
    }

    public async Task<Sprint?> GetActiveSprint()
    {
        return (await GetAll()).FirstOrDefault(s => s.IsActive);
    }

    public async Task AddToSprint(Guid sprintId, Guid ticketId)
    {
        var activeSprint = await GetById(sprintId);

        if (activeSprint == null) return;

        await sprintCommandsService.AddTicketToSprint(new AddTicketToSprintCommand(sprintId, ticketId));
    }
}