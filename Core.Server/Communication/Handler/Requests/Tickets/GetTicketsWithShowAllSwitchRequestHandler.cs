using Service.Server.CQRS.Requests.Tickets;
using Service.Server.Old.Services.Entities.Sprints;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Requests.Tickets;

public class GetTicketsWithShowAllSwitchRequestHandler(
    ITicketRequestsService ticketRequestsService,
    ISprintRequestsService sprintRequestsService)
    : IRequestHandler<GetTicketsWithShowAllSwitchRequest, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetTicketsWithShowAllSwitchRequest request,
        CancellationToken cancellationToken)
    {

    }
}