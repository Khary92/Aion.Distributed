using Proto.Notifications.Tag;
using Service.Admin.Web.Communication.Tags.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Tags;

public static class TagExtensions
{
    public static TagDto ToDto(this TagCreatedNotification notification)
    {
        return new TagDto(Guid.Parse(notification.TagId), notification.Name, false);
    }

    public static WebTagUpdatedNotification ToNotification(this TagUpdatedNotification notification)
    {
        return new WebTagUpdatedNotification(Guid.Parse(notification.TagId), notification.Name);
    }
}