using Service.Admin.Web.Communication.Tickets.Notifications;

namespace Service.Admin.Web.Models;

public class TicketWebModel
{
    public TicketWebModel(Guid ticketId, string name, string bookingNumber, string documentation, List<Guid> sprintIds)
    {
        TicketId = ticketId;
        Name = name;
        BookingNumber = bookingNumber;
        Documentation = documentation;
        SprintIds = sprintIds;
        PreviousDocumentation = documentation;
    }

    private string PreviousDocumentation { get; set; }

    public Guid TicketId { get; }

    public string Name { get; set; }

    public string BookingNumber { get; set; }

    public List<Guid> SprintIds { get; private set; }

    public string Documentation { get; set; }

    public void SynchronizeDocumentation(string documentation)
    {
        PreviousDocumentation = Documentation;
        Documentation = documentation;
    }

    public bool IsDocumentationChanged()
    {
        var result = !PreviousDocumentation.Equals(Documentation);
        PreviousDocumentation = Documentation;
        return result;
    }

    public void Apply(WebTicketDataUpdatedNotification notification)
    {
        BookingNumber = notification.BookingNumber;
        Name = notification.Name;
        SprintIds = notification.SprintIds;
    }

    public void Apply(WebTicketDocumentationUpdatedNotification notification)
    {
        Documentation = notification.Documentation;
    }
}