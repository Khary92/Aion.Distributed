using System;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Proto.Notifications.Tag;

namespace Client.Desktop.Communication.Notifications.Tag;

public static class TagExtensions
{
    public static NewTagMessage ToNewEntityMessage(this TagCreatedNotification notification)
    {
        return new NewTagMessage(new TagClientModel(Guid.Parse(notification.TagId), notification.Name, false));
    }

    public static ClientTagUpdatedNotification ToClientNotification(this TagUpdatedNotification notification)
    {
        return new ClientTagUpdatedNotification(Guid.Parse(notification.TagId), notification.Name);
    }
}