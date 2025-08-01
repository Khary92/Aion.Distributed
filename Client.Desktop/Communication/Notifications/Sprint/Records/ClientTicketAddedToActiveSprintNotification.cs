using System;

namespace Client.Desktop.Communication.Notifications.Sprint.Records;

public record ClientTicketAddedToActiveSprintNotification(Guid TicketId, Guid TraceId);