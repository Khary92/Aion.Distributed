using System;

namespace Client.Desktop.Communication.Requests.Ticket;

public record ClientGetTicketByIdRequest(Guid TicketId, Guid TraceId);