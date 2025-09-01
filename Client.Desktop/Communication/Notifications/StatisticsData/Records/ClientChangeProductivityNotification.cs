using System;

namespace Client.Desktop.Communication.Notifications.StatisticsData.Records;

public record ClientChangeProductivityNotification(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive,
    Guid TraceId);