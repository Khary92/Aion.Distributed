using System;

namespace Client.Desktop.Communication.Commands.WorkDays.Records;

public record ClientCreateWorkDayCommand(
    Guid WorkDayId,
    DateTimeOffset Date,
    Guid TraceId);