
namespace Service.Server.CQRS.Requests.Notes;

public record GetNotesByTimeSlotIdRequest(Guid TimeSlotId);