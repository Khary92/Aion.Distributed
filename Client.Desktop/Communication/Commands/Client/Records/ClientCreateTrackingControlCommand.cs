using System;

namespace Client.Desktop.Communication.Commands.Client.Records;

public record ClientCreateTrackingControlCommand(
    Guid TicketId,
    DateTimeOffset Date,
    Guid TraceId);