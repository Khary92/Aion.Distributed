using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Notes;

public record GetNotesByTimeSlotIdRequest(Guid TimeSlotId) : IRequest<List<NoteDto>>, INotification;