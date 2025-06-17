using System.Text.Json;
using Domain.Events.TimeSlots;

namespace Domain.Entities;

public class TimeSlot
{
    public Guid TimeSlotId { get; set; }
    public Guid SelectedTicketId { get; set; }
    public Guid WorkDayId { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public bool IsTimerRunning { get; set; }
    public List<Guid> NoteIds { get; private set; } = [];

    public static TimeSlot Rehydrate(IEnumerable<TimeSlotEvent> events)
    {
        var timeSlot = new TimeSlot();
        foreach (var evt in events) timeSlot.Apply(evt);

        return timeSlot;
    }

    private void Apply(TimeSlotEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(TimeSlotCreatedEvent):
                var created = JsonSerializer.Deserialize<TimeSlotCreatedEvent>(evt.EventPayload);
                TimeSlotId = created!.TimeSlotId;
                SelectedTicketId = created.SelectedTicketId;
                StartTime = created.StartTime;
                EndTime = created.EndTime;
                WorkDayId = created.WorkDayId;
                IsTimerRunning = created.IsTimerRunning;
                NoteIds = created.NoteIds;
                break;

            case nameof(NoteAddedEvent):
                var added = JsonSerializer.Deserialize<NoteAddedEvent>(evt.EventPayload);
                NoteIds.Add(added!.NoteId);
                break;

            case nameof(StartTimeSetEvent):
                var startTime = JsonSerializer.Deserialize<StartTimeSetEvent>(evt.EventPayload);
                StartTime = startTime!.Time;
                break;

            case nameof(EndTimeSetEvent):
                var endTime = JsonSerializer.Deserialize<EndTimeSetEvent>(evt.EventPayload);
                EndTime = endTime!.Time;
                break;
        }
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