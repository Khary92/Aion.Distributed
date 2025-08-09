using System;

namespace Client.Desktop.Communication.Commands.Ticket;

public record ClientUpdateTicketDocumentationCommand(
    Guid TicketId,
    string Documentation,
    Guid TraceId);