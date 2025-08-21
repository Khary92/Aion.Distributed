using System;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using Proto.Notifications.UseCase;

namespace Client.Desktop.Communication.Notifications.UseCase;

public static class UseCaseExtensions
{
    public static ClientSprintSelectionChangedNotification ToClientNotification(
        this SprintSelectionChangedNotification notification)
    {
        return new ClientSprintSelectionChangedNotification(Guid.Parse(notification.TraceData.TraceId));
    }
    
    public static ClientTimeSlotControlCreatedNotification ToClientNotification(
        this TimeSlotControlCreatedNotification notification)
    {
        return new ClientTimeSlotControlCreatedNotification(
            notification.TimeSlotControlData.StatisticsDataProto.ToClientModel(),
            notification.TimeSlotControlData.TicketProto.ToClientModel(),
            notification.TimeSlotControlData.TimeSlotProto.ToClientModel(),
            Guid.Parse(notification.TraceData.TraceId));
    }
}