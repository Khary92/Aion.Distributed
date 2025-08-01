using System;

namespace Client.Desktop.Communication.Requests.Ticket;

public record ClientGetTicketsWithShowAllSwitchRequest(bool IsShowAll, Guid TraceId);