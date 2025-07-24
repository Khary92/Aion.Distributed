using Proto.Notifications.Sprint;
using Service.Admin.Web.Communication.Sprints.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Sprints;

public static class SprintExtensions
{
    public static SprintDto ToDto(this SprintCreatedNotification notification)
        => new(Guid.Parse(notification.SprintId), notification.Name, notification.IsActive,
            notification.StartTime.ToDateTimeOffset(), notification.EndTime.ToDateTimeOffset(),
            notification.TicketIds.ToGuidList());

    public static WebSprintDataUpdatedNotification ToNotification(this SprintDataUpdatedNotification notification)
        => new(Guid.Parse(notification.SprintId), notification.Name, notification.StartTime.ToDateTimeOffset(),
            notification.EndTime.ToDateTimeOffset());
    
    public static WebSetSprintActiveStatusNotification ToNotification(this SprintActiveStatusSetNotification notification)
        => new(Guid.Parse(notification.SprintId), notification.IsActive);
    
    public static WebAddTicketToActiveSprintNotification ToNotification(this TicketAddedToActiveSprintNotification notification)
        => new(Guid.Parse(notification.TicketId));
    
    public static WebAddTicketToSprintNotification ToNotification(this TicketAddedToSprintNotification notification)
        => new(Guid.Parse(notification.SprintId), Guid.Parse(notification.TicketId));
}