using System;

namespace Client.Desktop.Communication.Commands.TimeSlots.Records;

public record ClientSetEndTimeCommand(Guid TimeSlotId, DateTimeOffset Time,
    Guid TraceId);