using System;

namespace Client.Desktop.Communication.Notifications.Ticket.Records;

public record ClientTicketDocumentationUpdatedNotification(Guid TicketId, string Documentation, Guid TraceId);