using System;
using Client.Desktop.Communication.Notifications.Client.Records;
using Proto.Notifications.Client;

namespace Client.Desktop.Communication.Notifications.Client;

public static class ClientExtensions
{
    public static ClientSprintSelectionChangedNotification ToClientNotification(
        this SprintSelectionChangedNotification notification)
    {
        return new ClientSprintSelectionChangedNotification();
    }

    public static ClientTrackingControlCreatedNotification ToClientNotification(
        this TrackingControlCreatedNotification notification)
    {
        return new ClientTrackingControlCreatedNotification(
            notification.TimeSlotControlData.StatisticsDataProto.ToClientModel(),
            notification.TimeSlotControlData.TicketProto.ToClientModel(),
            notification.TimeSlotControlData.TimeSlotProto.ToClientModel(),
            Guid.Parse(notification.TraceData.TraceId));
    }
}