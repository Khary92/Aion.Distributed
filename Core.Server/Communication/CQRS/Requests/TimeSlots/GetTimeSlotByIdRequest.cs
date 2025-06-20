namespace Core.Server.Communication.CQRS.Requests.TimeSlots;

public record GetTimeSlotByIdRequest(Guid TimeSlotId);