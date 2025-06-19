
namespace Service.Server.CQRS.Commands.Entities.TimeSlots;

public record SetStartTimeCommand(Guid TimeSlotId, DateTimeOffset Time);