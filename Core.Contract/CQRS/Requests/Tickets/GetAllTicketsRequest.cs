using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Tickets;

public record GetAllTicketsRequest : IRequest<List<TicketDto>>, INotification;