using Contract.CQRS.Notifications.UseCase;
using Proto.Notification.UseCase;

namespace Contract.Proto.Converter.Notifications;

public static class UseCaseNotificationConverter
{
    public static CreateSnapshotNotificationProto ToProto(CreateSnapshotNotification _)
        => new();

    public static CreateSnapshotNotification FromProto(CreateSnapshotNotificationProto _)
        => new();

    public static SaveDocumentationNotificationProto ToProto(SaveDocumentationNotification _)
        => new();

    public static SaveDocumentationNotification FromProto(SaveDocumentationNotificationProto _)
        => new();

    public static SprintSelectionChangedNotificationProto ToProto(SprintSelectionChangedNotification _)
        => new();

    public static SprintSelectionChangedNotification FromProto(SprintSelectionChangedNotificationProto _)
        => new();

    public static TimeSlotControlCreatedNotificationProto ToProto(TimeSlotControlCreatedNotification n)
        => new()
        {
            ViewId = n.ViewId.ToString(),
            TimeSlotId = n.TimeSlotId.ToString(),
            TicketId = n.TicketId.ToString()
        };

    public static TimeSlotControlCreatedNotification FromProto(TimeSlotControlCreatedNotificationProto p)
        => new(
            Guid.Parse(p.ViewId),
            Guid.Parse(p.TimeSlotId),
            Guid.Parse(p.TicketId)
        );

    public static TraceReportSentNotificationProto ToProto(TraceReportSentNotification n)
        => new() { TraceReportDto = n.TraceReportDto };

    public static TraceReportSentNotification FromProto(TraceReportSentNotificationProto p)
        => new(p.TraceReportDto);

    public static WorkDaySelectionChangedNotificationProto ToProto(WorkDaySelectionChangedNotification _)
        => new();

    public static WorkDaySelectionChangedNotification FromProto(WorkDaySelectionChangedNotificationProto _)
        => new();
}