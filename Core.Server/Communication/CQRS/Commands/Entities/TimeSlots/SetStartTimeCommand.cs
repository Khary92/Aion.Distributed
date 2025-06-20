namespace Core.Server.Communication.CQRS.Commands.Entities.TimeSlots;

public record SetStartTimeCommand(Guid TimeSlotId, DateTimeOffset Time);