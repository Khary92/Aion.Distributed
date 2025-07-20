namespace Service.Admin.Web.DTO;

public class SprintDto
{
    private readonly List<Guid> _ticketIds = [];
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
        init
        {
            _ticketIds.Clear();
            if (value != null)
                _ticketIds.AddRange(value);
        }
    }

    public void Apply(dynamic notification)
    {
        if (notification?.EndTime is DateTimeOffset end)
            EndTime = end;
        if (notification?.StartTime is DateTimeOffset start)
            StartTime = start;
        if (notification?.Name is string nameVal)
            Name = nameVal;
    }

    public void ApplyTicketAdded(dynamic notification)
    {
        var idString = (string)notification.TicketId;
        if (Guid.TryParse(idString, out var parsedGuid) && !TicketIds.Contains(parsedGuid))
        {
            TicketIds.Add(parsedGuid);
        }
    }

    public void ApplyActiveStatus(dynamic notification)
    {
        if (notification?.IsActive is bool status)
            IsActive = status;
    }

    public override string ToString()
    {
        return $"SprintDto:{{sprintId:'{SprintId}', Name:'{Name}', isActive:'{IsActive}', startTime:'{StartTime}', endTime:'{EndTime}', ticketIds:'{string.Join(",", TicketIds)}'}}";
    }
}