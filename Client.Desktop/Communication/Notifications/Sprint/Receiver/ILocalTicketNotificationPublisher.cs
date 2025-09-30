using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Client.Desktop.Communication.Notifications.Wrappers;

namespace Client.Desktop.Communication.Notifications.Sprint.Receiver;

public interface ILocalSprintNotificationPublisher
{
    event Func<ClientSprintActiveStatusSetNotification, Task>? ClientSprintActiveStatusSetNotificationReceived;
    event Func<ClientSprintDataUpdatedNotification, Task>? ClientSprintDataUpdatedNotificationReceived;
    event Func<ClientTicketAddedToActiveSprintNotification, Task>? ClientTicketAddedToActiveSprintNotificationReceived;
    event Func<NewSprintMessage, Task>? NewSprintMessageReceived;
}