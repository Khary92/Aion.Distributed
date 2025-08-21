using System;

namespace Client.Desktop.Communication.Notifications.TimerSettings.Records;

public record ClientDocuTimerSaveIntervalChangedNotification(Guid TimerSettingsId, int SaveInterval, Guid TraceId);