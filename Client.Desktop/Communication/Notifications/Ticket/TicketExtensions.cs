using System;
using Client.Desktop.Communication.Notifications.Ticket.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.Notifications.Ticket;

namespace Client.Desktop.Communication.Notifications.Ticket;

public static class TicketExtensions
{
    public static NewTicketMessage ToNewEntityMessage(this TicketCreatedNotification notification) => new(
        new TicketClientModel(Guid.Parse(notification.TicketId), notification.Name, notification.BookingNumber,
            string.Empty, notification.SprintIds.ToGuidList()), Guid.Parse(notification.TraceData.TraceId));


    public static ClientTicketDataUpdatedNotification
        ToClientNotification(this TicketDataUpdatedNotification notification) => new(Guid.Parse(notification.TicketId),
        notification.Name, notification.BookingNumber, notification.SprintIds.ToGuidList(),
        Guid.Parse(notification.TraceData.TraceId));


    public static ClientTicketDocumentationUpdatedNotification ToClientNotification(
        this TicketDocumentationUpdatedNotification notification) => new(Guid.Parse(notification.TicketId),
        notification.Documentation, Guid.Parse(notification.TraceData.TraceId));
}