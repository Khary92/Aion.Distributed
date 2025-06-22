namespace Core.Server.Communication.Records.Commands.Entities.TimeSlots;

public record SetEndTimeCommand(Guid TimeSlotId, DateTimeOffset Time);