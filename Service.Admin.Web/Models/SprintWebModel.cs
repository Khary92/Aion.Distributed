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

    public Guid SprintId { get; }

    public string Name { get; private set; }

    public DateTimeOffset StartTime { get; private set; }

    public DateTimeOffset EndTime { get; private set; }
    
    public bool IsActive { get; set; }

    public List<Guid> TicketIds { get; set; } = [];

    public void Apply(WebAddTicketToActiveSprintNotification notification)
    {
        TicketIds.Add(notification.TicketId);
    }
    
    public void Apply(WebSprintDataUpdatedNotification notification)
    {
        Name = notification.Name;
        StartTime = notification.StartTime;
        EndTime = notification.EndTime;
    }
}