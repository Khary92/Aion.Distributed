
namespace Application.Contract.CQRS.Requests.Notes;

public record GetNotesByTimeSlotIdRequest(Guid TimeSlotId);