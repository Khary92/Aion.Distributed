using Proto.Notifications.Sprint;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sprints;

public static class SprintExtensions
{
    public static NewSprintMessage ToWebModel(this SprintCreatedNotification notification) => new(new SprintWebModel(
        Guid.Parse(notification.SprintId), notification.Name, notification.IsActive,
        notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
        notification.TicketIds.ToGuidList()), Guid.Parse(notification.TraceData.TraceId));

    public static WebSprintDataUpdatedNotification ToNotification(this SprintDataUpdatedNotification notification)
        => new(Guid.Parse(notification.SprintId), notification.Name,
            notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
            Guid.Parse(notification.TraceData.TraceId));

    public static WebSetSprintActiveStatusNotification ToNotification(
        this SprintActiveStatusSetNotification notification) => new(
        Guid.Parse(notification.SprintId), notification.IsActive, Guid.Parse(notification.TraceData.TraceId));

    public static WebAddTicketToActiveSprintNotification ToNotification(
        this TicketAddedToActiveSprintNotification notification) => new(
        Guid.Parse(notification.TicketId), Guid.Parse(notification.TraceData.TraceId));

    public static WebAddTicketToSprintNotification ToNotification(this TicketAddedToSprintNotification notification) =>
        new(Guid.Parse(notification.SprintId), Guid.Parse(notification.TicketId),
            Guid.Parse(notification.TraceData.TraceId));
}