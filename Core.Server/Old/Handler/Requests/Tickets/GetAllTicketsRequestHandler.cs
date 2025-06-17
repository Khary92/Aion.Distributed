using Application.Contract.CQRS.Requests.Tickets;
using Application.Contract.DTO;
using Application.Services.Entities.Tickets;
using MediatR;

namespace Application.Handler.Requests.Tickets;

public class GetAllTicketsRequestHandler(ITicketRequestsService ticketRequestsService)
    : IRequestHandler<GetAllTicketsRequest, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetAllTicketsRequest request, CancellationToken cancellationToken)
    {
        return await ticketRequestsService.GetAll();
    }
}