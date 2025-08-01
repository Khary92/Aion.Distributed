using System;
using Client.Desktop.Communication.Notifications.UseCase.Records;
using Proto.Notifications.UseCase;

namespace Client.Desktop.Communication.Notifications.UseCase;

public static class UseCaseExtensions
{
    public static ClientWorkDaySelectionChangedNotification ToClientNotification(
        this WorkDaySelectionChangedNotification notification) =>
        new(Guid.Parse(notification.TraceData.TraceId));

    public static ClientSprintSelectionChangedNotification ToClientNotification(
        this SprintSelectionChangedNotification notification) =>
        new(Guid.Parse(notification.TraceData.TraceId));

    public static ClientSaveDocumentationNotification ToClientNotification(
        this SaveDocumentationNotification notification) =>
        new(Guid.Parse(notification.TraceData.TraceId));

    public static ClientCreateSnapshotNotification ToClientNotification(this CreateSnapshotNotification notification) =>
        new(Guid.Parse(notification.TraceData.TraceId));

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