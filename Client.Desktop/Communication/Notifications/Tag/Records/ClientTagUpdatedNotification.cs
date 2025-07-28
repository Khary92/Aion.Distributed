using System;

namespace Client.Desktop.Communication.Notifications.Tag.Records;

public record ClientTagUpdatedNotification(Guid TagId, string Name);