using System;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.Notifications.Sprint;

namespace Client.Desktop.Communication.Notifications.Sprint;

public static class SprintExtensions
{
    public static NewSprintMessage ToNewEntityMessage(this SprintCreatedNotification notification)
    {
        return new NewSprintMessage(
            new SprintClientModel(Guid.Parse(notification.SprintId), notification.Name, notification.IsActive,
                notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
                notification.TicketIds.ToGuidList()), Guid.Parse(notification.TraceData.TraceId));
    }

    public static ClientSprintActiveStatusSetNotification
        ToClientNotification(this SprintActiveStatusSetNotification notification)
    {
        return new ClientSprintActiveStatusSetNotification(Guid.Parse(notification.SprintId), notification.IsActive,
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static ClientSprintDataUpdatedNotification
        ToClientNotification(this SprintDataUpdatedNotification notification)
    {
        return new ClientSprintDataUpdatedNotification(Guid.Parse(notification.SprintId),
            notification.Name, notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
            Guid.Parse(notification.TraceData.TraceId));
    }

    public static ClientTicketAddedToActiveSprintNotification ToClientNotification(
        this TicketAddedToActiveSprintNotification notification)
    {
        return new ClientTicketAddedToActiveSprintNotification(Guid.Parse(notification.TicketId),
            Guid.Parse(notification.TraceData.TraceId));
    }
}