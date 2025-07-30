using System;

namespace Client.Desktop.Communication.Commands.UseCases.Records;

public record ClientCreateTimeSlotControlCommand(Guid TicketId,
    Guid TraceId);