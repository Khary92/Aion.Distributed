using Proto.Notifications.Ticket;
using Service.Admin.Web.Communication.Tickets.Notifications;

namespace Service.Admin.Web.DTO;

public class TicketDto
{
    private readonly Guid _ticketId;
    private string _bookingNumber;
    private string _documentation;
    private string _name;
    private List<Guid> _sprintIds;

    public TicketDto(Guid ticketId, string name, string bookingNumber, string documentation, List<Guid> sprintIds)
    {
        _ticketId = ticketId;
        _name = name;
        _bookingNumber = bookingNumber;
        _documentation = documentation;
        _sprintIds = sprintIds;
        PreviousDocumentation = documentation;
    }

    private string PreviousDocumentation { get; set; }

    public Guid TicketId
    {
        get => _ticketId;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string BookingNumber
    {
        get => _bookingNumber;
        set => _bookingNumber = value;
    }

    public List<Guid> SprintIds
    {
        get => _sprintIds;
        private set => _sprintIds = value;
    }

    public string Documentation
    {
        get => _documentation;
        set => _documentation = value;
    }

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