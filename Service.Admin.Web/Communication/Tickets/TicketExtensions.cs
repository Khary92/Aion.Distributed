using Proto.Notifications.Ticket;
using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tickets;

public static class TicketExtensions
{
    public static TicketWebModel ToDto(this TicketCreatedNotification notification)
    {
        return new TicketWebModel(Guid.Parse(notification.TicketId), notification.Name, notification.BookingNumber,
            string.Empty, notification.SprintIds.ToGuidList());
    }

    public static WebTicketDataUpdatedNotification ToNotification(this TicketDataUpdatedNotification notification)
    {
        return new WebTicketDataUpdatedNotification(Guid.Parse(notification.TicketId), notification.Name,
            notification.BookingNumber, notification.SprintIds.ToGuidList());
    }

    public static WebTicketDocumentationUpdatedNotification ToNotification(
        this TicketDocumentationUpdatedNotification notification)
    {
        return new WebTicketDocumentationUpdatedNotification(Guid.Parse(notification.TicketId),
            notification.Documentation);
    }
}