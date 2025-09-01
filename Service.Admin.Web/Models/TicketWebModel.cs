using Service.Admin.Web.Communication.Tickets.Notifications;

namespace Service.Admin.Web.Models;

public class TicketWebModel(
    Guid ticketId,
    string name,
    string bookingNumber,
    List<Guid> sprintIds)
{
    public Guid TicketId { get; } = ticketId;

    public string Name { get; set; } = name;

    public string BookingNumber { get; set; } = bookingNumber;

    public List<Guid> SprintIds { get; private set; } = sprintIds;

    public void Apply(WebTicketDataUpdatedNotification notification)
    {
        BookingNumber = notification.BookingNumber;
        Name = notification.Name;
        SprintIds = notification.SprintIds;
    }

    public void Apply(WebTicketDocumentationUpdatedNotification notification)
    {
        // Not required yet. This is a dummy for consistency reasons.
    }
}