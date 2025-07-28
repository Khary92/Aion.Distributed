using Proto.Notifications.Sprint;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Sprints;

public static class SprintExtensions
{
    public static SprintWebModel ToWebModel(this SprintCreatedNotification notification)
    {
        return new SprintWebModel(Guid.Parse(notification.SprintId), notification.Name, notification.IsActive,
            notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
            notification.TicketIds.ToGuidList());
    }

    public static WebSprintDataUpdatedNotification ToNotification(this SprintDataUpdatedNotification notification)
    {
        return new WebSprintDataUpdatedNotification(Guid.Parse(notification.SprintId), notification.Name,
            notification.StartTime.ToDateTimeOffset(),
            notification.EndTime.ToDateTimeOffset());
    }

    public static WebSetSprintActiveStatusNotification ToNotification(
        this SprintActiveStatusSetNotification notification)
    {
        return new WebSetSprintActiveStatusNotification(Guid.Parse(notification.SprintId), notification.IsActive);
    }

    public static WebAddTicketToActiveSprintNotification ToNotification(
        this TicketAddedToActiveSprintNotification notification)
    {
        return new WebAddTicketToActiveSprintNotification(Guid.Parse(notification.TicketId));
    }

    public static WebAddTicketToSprintNotification ToNotification(this TicketAddedToSprintNotification notification)
    {
        return new WebAddTicketToSprintNotification(Guid.Parse(notification.SprintId),
            Guid.Parse(notification.TicketId));
    }
}