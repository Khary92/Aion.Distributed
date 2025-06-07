using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Replays;

public record GetTicketReplayByIdRequest(Guid TicketId) : INotification, IRequest<TicketReplayDecorator>;