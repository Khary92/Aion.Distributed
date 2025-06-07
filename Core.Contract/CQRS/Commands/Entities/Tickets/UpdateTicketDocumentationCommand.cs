using MediatR;

namespace Contract.CQRS.Commands.Entities.Tickets;

public record UpdateTicketDocumentationCommand(Guid TicketId, string Documentation) : IRequest<Unit>, INotification;