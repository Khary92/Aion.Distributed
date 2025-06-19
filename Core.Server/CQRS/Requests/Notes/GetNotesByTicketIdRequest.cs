
namespace Service.Server.CQRS.Requests.Notes;

public record GetNotesByTicketIdRequest(Guid TicketId);