using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.Notifications.Ticket.Records;

public record ClientTicketDataUpdatedNotification(
    Guid TicketId,
    string Name,
    string BookingNumber,
    List<Guid> SprintIds,
    Guid TraceId);