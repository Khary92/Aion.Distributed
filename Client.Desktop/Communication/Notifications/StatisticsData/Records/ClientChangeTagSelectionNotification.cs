using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.Notifications.StatisticsData.Records;

public record ClientChangeTagSelectionNotification(
    Guid StatisticsDataId,
    List<Guid> SelectedTagIds,
    Guid TraceId);