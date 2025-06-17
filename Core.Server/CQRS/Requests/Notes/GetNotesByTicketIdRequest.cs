
namespace Application.Contract.CQRS.Requests.Notes;

public record GetNotesByTicketIdRequest(Guid TicketId);