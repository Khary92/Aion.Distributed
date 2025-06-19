
namespace Service.Server.CQRS.Commands.Entities.TimeSlots;

public record SetEndTimeCommand(Guid TimeSlotId, DateTimeOffset Time);