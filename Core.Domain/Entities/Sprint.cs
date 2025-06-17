using System.Text.Json;
using Domain.Events.Sprints;

namespace Domain.Entities;

public class Sprint
{
    public Guid SprintId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> TicketIds { get; set; } = [];

    public static Sprint Rehydrate(IEnumerable<SprintEvent> events)
    {
        var sprint = new Sprint();
        foreach (var evt in events) sprint.Apply(evt);
        return sprint;
    }

    private void Apply(SprintEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(SprintCreatedEvent):
                var created = JsonSerializer.Deserialize<SprintCreatedEvent>(evt.EventPayload);
                SprintId = created!.SprintId;
                Name = created.Name;
                StartDate = created.StartDate;
                EndDate = created.EndDate;
                IsActive = created.IsActive;
                TicketIds = created.TicketIds;
                break;

            case nameof(SprintDataUpdatedEvent):
                var updated = JsonSerializer.Deserialize<SprintDataUpdatedEvent>(evt.EventPayload);
                Name = updated!.Name;
                StartDate = updated.StartDate;
                EndDate = updated.EndDate;
                break;

            case nameof(TicketAddedToSprintEvent):
                var ticketAdded = JsonSerializer.Deserialize<TicketAddedToSprintEvent>(evt.EventPayload);
                TicketIds.Add(ticketAdded!.TicketId);
                break;

            case nameof(SprintActiveStatusChangedEvent):
                var status = JsonSerializer.Deserialize<SprintActiveStatusChangedEvent>(evt.EventPayload);
                IsActive = status!.IsActive;
                break;
        }
    }
}