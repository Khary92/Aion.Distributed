using Service.Server.CQRS.Requests.Tickets;
using Service.Server.Old.Services.Entities.Tickets;

namespace Service.Server.Old.Handler.Requests.Tickets;

public class GetAllTicketsRequestHandler(ITicketRequestsService ticketRequestsService)
    : IRequestHandler<GetAllTicketsRequest, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetAllTicketsRequest request, CancellationToken cancellationToken)
    {
        return await ticketRequestsService.GetAll();
    }
}