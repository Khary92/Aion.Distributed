using Service.Admin.Web.Communication.Sprints.Notifications;

namespace Service.Admin.Web.Models;

public class SprintWebModel
{
    public SprintWebModel(Guid sprintId, string name, bool isActive, DateTimeOffset startTime, DateTimeOffset endTime,
        List<Guid> ticketIds)
    {
        SprintId = sprintId;
        Name = name;
        IsActive = isActive;
        StartTime = startTime;
        EndTime = endTime;
        TicketIds = ticketIds;
    }

    public Guid SprintId { get; set; }

    public string Name { get; private set; } = string.Empty;

    public DateTimeOffset StartTime { get; private set; }

    public DateTimeOffset EndTime { get; private set; }

    public string StartTimeRepresentation => StartTime.ToLocalTime().ToString("dd.MM.yyyy");

    public string EndTimeRepresentation => EndTime.ToLocalTime().ToString("dd.MM.yyyy");

    public bool IsActive { get; set; }

    public List<Guid> TicketIds { get; set; } = [];

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