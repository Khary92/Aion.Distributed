using System;
using Client.Desktop.Communication.Notifications.StatisticsData.Records;
using Client.Proto;
using Proto.Notifications.StatisticsData;

namespace Client.Desktop.Communication.Notifications.StatisticsData;

public static class StatisticsDataExtensions
{
    public static ClientChangeProductivityNotification
        ToClientNotification(this ChangeProductivityNotification proto) =>
        new(Guid.Parse(proto.StatisticsDataId), proto.IsProductive, proto.IsNeutral, proto.IsUnproductive,
            Guid.Parse(proto.TraceData.TraceId));

    public static ClientChangeTagSelectionNotification
        ToClientNotification(this ChangeTagSelectionNotification proto) => new(Guid.Parse(proto.StatisticsDataId),
        proto.SelectedTagIds.ToGuidList(), Guid.Parse(proto.TraceData.TraceId));
}