
namespace Service.Server.Communication.CQRS.Commands.Entities.Tickets;

public record UpdateTicketDocumentationCommand(Guid TicketId, string Documentation);