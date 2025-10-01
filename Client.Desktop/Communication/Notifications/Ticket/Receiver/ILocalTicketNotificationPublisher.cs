using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;

namespace Client.Desktop.Communication.Notifications.Ticket.Receiver;

public interface ILocalTicketNotificationPublisher
{
    event Func<ClientTicketDataUpdatedNotification, Task>? TicketDataUpdatedNotificationReceived;
    event Func<ClientTicketDocumentationUpdatedNotification, Task>? TicketDocumentationUpdatedNotificationReceived;
    event Func<NewTicketMessage, Task>? NewTicketNotificationReceived;
    Task Publish(NewTicketMessage message);
    Task Publish(ClientTicketDataUpdatedNotification notification);
    Task Publish(ClientTicketDocumentationUpdatedNotification notification);
}