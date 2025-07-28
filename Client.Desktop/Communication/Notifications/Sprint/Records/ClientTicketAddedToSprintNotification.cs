using System;

namespace Client.Desktop.Communication.Notifications.Sprint.Records;

public record ClientTicketAddedToSprintNotification(Guid SprintId, Guid TicketId);