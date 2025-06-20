namespace Core.Server.Communication.CQRS.Commands.Entities.TimeSlots;

public record SetEndTimeCommand(Guid TimeSlotId, DateTimeOffset Time);