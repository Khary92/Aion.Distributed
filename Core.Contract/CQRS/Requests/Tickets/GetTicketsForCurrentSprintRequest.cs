using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Tickets;

public record GetTicketsForCurrentSprintRequest : IRequest<List<TicketDto>>, INotification;