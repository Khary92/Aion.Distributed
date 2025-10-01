using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Wrappers;

namespace Client.Desktop.Communication.Notifications.WorkDay.Receiver;

public interface ILocalWorkDayNotificationPublisher
{
    event Func<NewWorkDayMessage, Task>? NewWorkDayMessageReceived;

    Task Publish(NewWorkDayMessage message);
}