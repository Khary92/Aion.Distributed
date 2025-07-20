using Service.Admin.Web.Communication.Sprints.Notifications;

namespace Service.Admin.Web.DTO;

public class SprintDto
{
    private List<Guid> _ticketIds = [];
    private DateTimeOffset _endTime;
    private bool _isActive;
    private string _name = string.Empty;
    private DateTimeOffset _startTime;

    public SprintDto(Guid sprintId, string name, bool isActive, DateTimeOffset startTime, DateTimeOffset endTime, List<Guid> ticketIds)
    {
        SprintId = sprintId;
        Name = name;
        IsActive = isActive;
        StartTime = startTime;
        EndTime = endTime;
        TicketIds = ticketIds;
    }

    public Guid SprintId { get; set; }

    public string Name
    {
        get => _name;
        private set => _name = value;
    }

    public DateTimeOffset StartTime
    {
        get => _startTime;
        private set => _startTime = value;
    }

    public DateTimeOffset EndTime
    {
        get => _endTime;
        private set => _endTime = value;
    }

    public string StartTimeRepresentation => StartTime.ToLocalTime().ToString("dd.MM.yyyy");

    public string EndTimeRepresentation => EndTime.ToLocalTime().ToString("dd.MM.yyyy");

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public List<Guid> TicketIds
    {
        get => _ticketIds;
        set => _ticketIds = value;
    }

    // TODO: Seriously check if these two Notifications are really required this way
    public void Apply(WebAddTicketToActiveSprintNotification notification)
    {
        TicketIds.Add(notification.TicketId);
    }

    // TODO: Seriously check if these two Notifications are really required this way
    public void Apply(WebAddTicketToSprintNotification notification)
    {
        TicketIds.Add(notification.TicketId);
    }

    public void Apply(WebSetSprintActiveStatusNotification notification)
    {
        IsActive = notification.IsActive;
    }

    public void Apply(WebSprintDataUpdatedNotification notification)
    {
        Name = notification.Name;
        StartTime = notification.StartTime;
        EndTime = notification.EndTime;
    }
}