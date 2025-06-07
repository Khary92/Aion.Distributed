using Contract.Notifications.Entities.Sprints;
using ReactiveUI;

namespace Contract.DTO;

public class SprintDto : ReactiveObject
{
    private Guid _sprintId;
    private string _name = string.Empty;
    private bool _isActive;
    private DateTimeOffset _startTime;
    private DateTimeOffset _endTime;
    private List<Guid> _ticketIds = [];

    public SprintDto(Guid sprintId, string name, bool isActive, DateTimeOffset startTime, DateTimeOffset endTime,
        List<Guid> ticketIds)
    {
        SprintId = sprintId;
        Name = name;
        IsActive = isActive;
        StartTime = startTime;
        EndTime = endTime;
        TicketIds = ticketIds;
    }

    public Guid SprintId
    {
        get => _sprintId;
        set => this.RaiseAndSetIfChanged(ref _sprintId, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }


    public DateTimeOffset StartTime
    {
        get => _startTime;
        private set
        {
            this.RaiseAndSetIfChanged(ref _startTime, value);
            this.RaisePropertyChanged(nameof(StartTimeRepresentation));
        }
    }

    public DateTimeOffset EndTime
    {
        get => _endTime;
        private set
        {
            this.RaiseAndSetIfChanged(ref _endTime, value);
            this.RaisePropertyChanged(nameof(EndTimeRepresentation));
        }
    }

    public string StartTimeRepresentation => StartTime.ToLocalTime().ToString("dd.MM.yyyy");

    public string EndTimeRepresentation => EndTime.ToLocalTime().ToString("dd.MM.yyyy");

    public bool IsActive
    {
        get => _isActive;
        set => this.RaiseAndSetIfChanged(ref _isActive, value);
    }

    public List<Guid> TicketIds
    {
        get => _ticketIds;
        init => this.RaiseAndSetIfChanged(ref _ticketIds, value);
    }

    public void Apply(SprintDataUpdatedNotification notification)
    {
        EndTime = notification.EndTime;
        StartTime = notification.StartTime;
        Name = notification.Name;
    }

    public void Apply(TicketAddedToSprintNotification notification)
    {
        if (!TicketIds.Contains(notification.TicketId))
            TicketIds.Add(notification.TicketId);
    }

    public void Apply(SprintActiveStatusSetNotification notification)
    {
        IsActive = notification.IsActive;
    }
    
    public override string ToString()
    {
        return $"SprintDto:{{sprintId:'{SprintId}', Name:'{Name}', isActive:'{IsActive}'," +
               $" startTime:'{StartTime}', endTime:'{EndTime}', ticketIds:'{TicketIds}'}}";
    }
}