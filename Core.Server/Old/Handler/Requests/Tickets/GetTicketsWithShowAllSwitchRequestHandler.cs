using Application.Contract.CQRS.Requests.Tickets;
using Application.Contract.DTO;
using Application.Services.Entities.Sprints;
using Application.Services.Entities.Tickets;
using MediatR;

namespace Application.Handler.Requests.Tickets;

public class GetTicketsWithShowAllSwitchRequestHandler(
    ITicketRequestsService ticketRequestsService,
    ISprintRequestsService sprintRequestsService)
    : IRequestHandler<GetTicketsWithShowAllSwitchRequest, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetTicketsWithShowAllSwitchRequest request,
        CancellationToken cancellationToken)
    {
        var allTickets = await ticketRequestsService.GetAll();
        if (request.IsShowAll) return allTickets;

        var currentSprint = (await sprintRequestsService.GetAll()).FirstOrDefault(s => s.IsActive);

        return currentSprint == null
            ? []
            : allTickets.Where(t => t.SprintIds.Contains(currentSprint.SprintId)).ToList();
    }
}