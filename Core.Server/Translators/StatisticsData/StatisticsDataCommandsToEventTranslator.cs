using System.Text.Json;
using Domain.Events.StatisticsData;
using Service.Server.CQRS.Commands.Entities.StatisticsData;

namespace Service.Server.Old.Translators.StatisticsData;

public class StatisticsDataCommandsToEventTranslator : IStatisticsDataCommandsToEventTranslator
{
    public StatisticsDataEvent ToEvent(CreateStatisticsDataCommand createStatisticsDataCommand)
    {
        var domainEvent = new StatisticsDataCreatedEvent(createStatisticsDataCommand.StatisticsDataId,
            createStatisticsDataCommand.IsProductive, createStatisticsDataCommand.IsNeutral,
            createStatisticsDataCommand.IsUnproductive, createStatisticsDataCommand.TagIds,
            createStatisticsDataCommand.TimeSlotId);

        return CreateDatabaseEvent(nameof(StatisticsDataCreatedEvent), createStatisticsDataCommand.StatisticsDataId,
            JsonSerializer.Serialize(domainEvent));
    }

    public StatisticsDataEvent ToEvent(ChangeProductivityCommand changeProductivityCommand)
    {
        var domainEvent = new ProductivityChangedEvent(changeProductivityCommand.StatisticsDataId,
            changeProductivityCommand.IsProductive, changeProductivityCommand.IsNeutral,
            changeProductivityCommand.IsUnproductive);

        return CreateDatabaseEvent(nameof(ProductivityChangedEvent), changeProductivityCommand.StatisticsDataId,
            JsonSerializer.Serialize(domainEvent));
    }

    public StatisticsDataEvent ToEvent(ChangeTagSelectionCommand changeTagSelectionCommand)
    {
        var domainEvent = new TagSelectionChangedEvent(changeTagSelectionCommand.StatisticsDataId,
            changeTagSelectionCommand.SelectedTagIds);

        return CreateDatabaseEvent(nameof(TagSelectionChangedEvent), changeTagSelectionCommand.StatisticsDataId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static StatisticsDataEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new StatisticsDataEvent(Guid.NewGuid(), DateTime.UtcNow,
            TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow), eventName,
            entityId, json);
    }
}