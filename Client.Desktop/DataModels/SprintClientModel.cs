using System;
using System.Collections.Generic;
using Client.Desktop.Communication.Notifications.Sprint.Records;
using Proto.Command.Sprints;
using Proto.Notifications.Sprint;
using ReactiveUI;

namespace Client.Desktop.DataModels;

public class SprintClientModel : ReactiveObject
{
    private readonly Guid _sprintId;
    private DateTimeOffset _endTime;
    private bool _isActive;
    private string _name = string.Empty;
    private DateTimeOffset _startTime;
    private List<Guid> _ticketIds = [];

    public SprintClientModel(Guid sprintId, string name, bool isActive, DateTimeOffset startTime,
        DateTimeOffset endTime,
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
        private init => this.RaiseAndSetIfChanged(ref _sprintId, value);
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
        private set => this.RaiseAndSetIfChanged(ref _isActive, value);
    }

    public List<Guid> TicketIds
    {
        get => _ticketIds;
        set => this.RaiseAndSetIfChanged(ref _ticketIds, value);
    }

    public void Apply(ClientSprintDataUpdatedNotification notification)
    {
        EndTime = notification.EndTime;
        StartTime = notification.StartTime;
        Name = notification.Name;
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

    public void Apply(UpdateSprintDataCommandProto notification)
    {
        Name = notification.Name;
        StartTime = notification.StartTime.ToDateTimeOffset();
        EndTime = notification.EndTime.ToDateTimeOffset();
    }
}