using Application.Contract.CQRS.Requests.Tickets;
using Application.Contract.DTO;
using Application.Services.Entities.Sprints;
using Application.Services.Entities.Tickets;
using MediatR;

namespace Application.Handler.Requests.Tickets;

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