using System;

namespace Client.Desktop.Communication.Notifications.TimerSettings.Records;

public record ClientSnapshotSaveIntervalChangedNotification(Guid TimerSettingsId, int SaveInterval, Guid TraceId);