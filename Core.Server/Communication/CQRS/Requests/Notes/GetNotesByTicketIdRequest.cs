
namespace Service.Server.Communication.CQRS.Requests.Notes;

public record GetNotesByTicketIdRequest(Guid TicketId);