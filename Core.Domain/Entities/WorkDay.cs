using System.Text.Json;
using Domain.Events.WorkDays;

namespace Domain.Entities;

public class WorkDay
{
    public Guid WorkDayId { get; set; }
    public DateTimeOffset Date { get; set; }

    public static WorkDay Rehydrate(IEnumerable<WorkDayEvent> events)
    {
        var workDay = new WorkDay();
        foreach (var evt in events) workDay.Apply(evt);

        return workDay;
    }

    private void Apply(WorkDayEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(WorkDayCreatedEvent):
                var created = JsonSerializer.Deserialize<WorkDayCreatedEvent>(evt.EventPayload);
                WorkDayId = created!.WorkDayId;
                Date = created.Date;
                break;

            case nameof(WorkDayUpdatedEvent):
                var updated = JsonSerializer.Deserialize<WorkDayUpdatedEvent>(evt.EventPayload);
                Date = updated!.Date;
                break;
        }
    }
}