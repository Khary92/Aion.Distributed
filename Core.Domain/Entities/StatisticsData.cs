using System.Text.Json;
using Domain.Events.StatisticsData;

namespace Domain.Entities;

public class StatisticsData
{
    public Guid StatisticsId { get; set; }
    public bool IsProductive { get; set; } 
    public bool IsNeutral { get; set; } 
    public bool IsUnproductive { get; set; } 
    public List<Guid> TagIds { get; set; } = [];
    public Guid TimeSlotId { get; set; }

    public static StatisticsData Rehydrate(IEnumerable<StatisticsDataEvent> events)
    {
        var statisticData = new StatisticsData();
        foreach (var evt in events) statisticData.Apply(evt);
        return statisticData;
    }

    private void Apply(StatisticsDataEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(StatisticsDataCreatedEvent):
                var created = JsonSerializer.Deserialize<StatisticsDataCreatedEvent>(evt.EventPayload);
                StatisticsId = created!.StatisticsDataId;
                IsProductive = created.IsProductive;
                IsNeutral = created.IsNeutral;
                IsUnproductive = created.IsUnproductive;
                TagIds = created.TagIds;
                TimeSlotId = created.TimeSlotId;
                break;

            case nameof(ProductivityChangedEvent):
                var updatedProductivity = JsonSerializer.Deserialize<ProductivityChangedEvent>(evt.EventPayload);
                IsProductive = updatedProductivity!.IsProductive;
                IsNeutral = updatedProductivity.IsNeutral;
                IsUnproductive = updatedProductivity.IsUnproductive;
                break;

            case nameof(TagSelectionChangedEvent):
                var updatedTagSelection = JsonSerializer.Deserialize<TagSelectionChangedEvent>(evt.EventPayload);
                TagIds = updatedTagSelection!.SelectedTagIds;
                break;
        }
    }
}