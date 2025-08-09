using System;

namespace Client.Desktop.Communication.Commands.TimeSlots.Records;

public record ClientSetStartTimeCommand(
    Guid TimeSlotId,
    DateTimeOffset Time,
    Guid TraceId);