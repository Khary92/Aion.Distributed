namespace Core.Server.Communication.CQRS.Requests.Notes;

public record GetNotesByTimeSlotIdRequest(Guid TimeSlotId);