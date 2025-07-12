using Proto.Notifications.Ticket;
using Service.Admin.Web.Communication.Tickets.Commands;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Tickets;

public static class TicketExtensions
{
    public static TicketDto ToDto(this TicketCreatedNotification notification)
    {
        return new TicketDto(Guid.Parse(notification.TicketId), notification.Name, notification.BookingNumber,
            string.Empty, notification.SprintIds.ToGuidList());
    }

    public static WebTicketDataUpdatedNotification ToCommand(this TicketDataUpdatedNotification notification)
    {
        return new WebTicketDataUpdatedNotification(Guid.Parse(notification.TicketId), notification.Name,
            notification.BookingNumber, notification.SprintIds.ToGuidList());
    }

    public static WebTicketDocumentationUpdatedNotification ToCommand(
        this TicketDocumentationUpdatedNotification notification)
    {
        return new WebTicketDocumentationUpdatedNotification(Guid.Parse(notification.TicketId),
            notification.Documentation);
    }
}