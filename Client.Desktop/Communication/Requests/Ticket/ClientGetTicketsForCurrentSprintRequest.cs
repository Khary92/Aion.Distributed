using System;

namespace Client.Desktop.Communication.Requests.Ticket;

public record ClientGetTicketsForCurrentSprintRequest(Guid TraceId);