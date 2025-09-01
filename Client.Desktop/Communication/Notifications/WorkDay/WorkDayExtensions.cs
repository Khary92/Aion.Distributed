using System;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Proto.Notifications.WorkDay;

namespace Client.Desktop.Communication.Notifications.WorkDay;

public static class WorkDayExtensions
{
    public static NewWorkDayMessage ToNewEntityMessage(this WorkDayCreatedNotification notification)
    {
        return new NewWorkDayMessage(new WorkDayClientModel(Guid.Parse(notification.WorkDayId),
            notification.Date.ToDateTimeOffset()));
    }
}