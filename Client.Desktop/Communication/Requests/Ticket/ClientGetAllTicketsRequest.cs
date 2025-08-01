using System;

namespace Client.Desktop.Communication.Requests.Ticket;

public record ClientGetAllTicketsRequest(Guid TraceId);