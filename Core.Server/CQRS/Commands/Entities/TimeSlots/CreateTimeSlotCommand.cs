
namespace Service.Server.CQRS.Commands.Entities.TimeSlots;

public record CreateTimeSlotCommand(
    Guid TimeSlotId,
    Guid SelectedTicketId,
    Guid WorkDayId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    bool IsTimerRunning);