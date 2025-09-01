using System;

namespace Client.Desktop.Communication.Commands.TimeSlots.Records;

public record ClientCreateTimeSlotCommand(
    Guid TimeSlotId,
    Guid SelectedTicketId,
    Guid WorkDayId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    bool IsTimerRunning);