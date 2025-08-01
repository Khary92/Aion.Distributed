using System;

namespace Client.Desktop.Communication.Notifications.Sprint.Records;

public record ClientSprintActiveStatusSetNotification(Guid SprintId, bool IsActive, Guid TraceId);