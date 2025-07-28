using Client.Desktop.Communication.Notifications.UseCase.Records;
using Proto.Notifications.UseCase;

namespace Client.Desktop.Communication.Notifications.UseCase;

public static class UseCaseExtensions
{
    public static ClientTimeSlotControlCreatedNotification ToClientNotification(
        this TimeSlotControlCreatedNotification notification)
    {
        return new ClientTimeSlotControlCreatedNotification(
            notification.TimeSlotControlData.StatisticsDataProto.ToModel(),
            notification.TimeSlotControlData.TicketProto.ToModel(),
            notification.TimeSlotControlData.TimeSlotProto.ToModel());
    }
}