using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Client.Desktop.DTO;

public class TimeSlotDto : ReactiveObject
{
    private readonly Guid _selectedTicket;
    private readonly Guid _timeSlotId;
    private readonly Guid _workDayId;
    private DateTimeOffset _endTime;
    private bool _isTimerRunning;

    private List<Guid> _noteIds = [];

    private DateTimeOffset _startTime;

    public TimeSlotDto(Guid timeSlotId, Guid workDayId, Guid ticketId, DateTimeOffset startTime, DateTimeOffset endTime,
        List<Guid> noteIds, bool isTimerRunning)
    {
        TimeSlotId = timeSlotId;
        WorkDayId = workDayId;
        SelectedTicketId = ticketId;
        StartTime = startTime;
        EndTime = endTime;
        NoteIds = noteIds;
        IsTimerRunning = isTimerRunning;

        PreviousEndTime = _endTime;
        PreviousStartTime = _startTime;
    }

    private DateTimeOffset PreviousEndTime { get; set; }
    private DateTimeOffset PreviousStartTime { get; set; }

    public Guid TimeSlotId
    {
        get => _timeSlotId;
        private init => this.RaiseAndSetIfChanged(ref _timeSlotId, value);
    }

    public Guid SelectedTicketId
    {
        get => _selectedTicket;
        private init => this.RaiseAndSetIfChanged(ref _selectedTicket, value);
    }

    public List<Guid> NoteIds
    {
        set => this.RaiseAndSetIfChanged(ref _noteIds, value);
    }

    public DateTimeOffset StartTime
    {
        get => _startTime;
        set => this.RaiseAndSetIfChanged(ref _startTime, value);
    }

    public DateTimeOffset EndTime
    {
        get => _endTime;
        set => this.RaiseAndSetIfChanged(ref _endTime, value);
    }

    public Guid WorkDayId
    {
        get => _workDayId;
        private init => this.RaiseAndSetIfChanged(ref _workDayId, value);
    }


    public bool IsTimerRunning
    {
        get => _isTimerRunning;
        set => this.RaiseAndSetIfChanged(ref _isTimerRunning, value);
    }

    public string GetElapsedTime()
    {
        var diff = EndTime - StartTime;

        return $"Elapsed time: {(int)diff.TotalHours:D2}:{diff.Minutes:D2}:{diff.Seconds:D2}";
    }

    public bool IsStartTimeChanged()
    {
        var result = PreviousStartTime != StartTime;
        PreviousStartTime = StartTime;
        return result;
    }

    public bool IsEndTimeChanged()
    {
        var result = PreviousEndTime != EndTime;
        PreviousEndTime = EndTime;
        return result;
    }

    public int GetDurationInSeconds()
    {
        var difference = EndTime - StartTime;
        return (int)difference.TotalSeconds;
    }

    public int GetDurationInMinutes()
    {
        var difference = EndTime - StartTime;
        return (int)difference.TotalMinutes;
    }
}