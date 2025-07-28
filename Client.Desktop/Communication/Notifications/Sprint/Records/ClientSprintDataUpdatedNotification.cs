using System;

namespace Client.Desktop.Communication.Notifications.Sprint.Records;

public record ClientSprintDataUpdatedNotification(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);