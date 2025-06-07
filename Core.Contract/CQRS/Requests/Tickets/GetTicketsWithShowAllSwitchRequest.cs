using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Tickets;

public record GetTicketsWithShowAllSwitchRequest(bool IsShowAll) : IRequest<List<TicketDto>>, INotification;