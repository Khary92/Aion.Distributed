using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Notes;

public record GetNotesByTicketIdRequest(Guid TicketId) : IRequest<List<NoteDto>>, INotification;