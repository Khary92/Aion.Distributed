
namespace Service.Server.CQRS.Commands.Entities.Tickets;

public record UpdateTicketDocumentationCommand(Guid TicketId, string Documentation);