namespace Core.Server.Communication.Records.Commands.Entities.TimeSlots;

public record SetStartTimeCommand(Guid TimeSlotId, DateTimeOffset Time);