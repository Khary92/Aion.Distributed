using Service.Server.CQRS.Requests.Tickets;
using Service.Server.Old.Services.Entities.Sprints;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Requests.Tickets;

public class GetTicketsForCurrentSprintRequestHandler(
    ITicketRequestsService ticketRequestsService,
    ISprintRequestsService sprintRequestsService)
    : IRequestHandler<GetTicketsForCurrentSprintRequest, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetTicketsForCurrentSprintRequest request,
        CancellationToken cancellationToken)
    {
        var currentSprint = (await sprintRequestsService.GetAll()).FirstOrDefault(t => t.IsActive);

        if (currentSprint == null) return [];

        var allTickets = await ticketRequestsService.GetAll();

        return allTickets.Where(t => t.SprintIds.Contains(currentSprint.SprintId)).ToList();
    }
}