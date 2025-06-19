using Domain.Events.StatisticsData;
using Domain.Interfaces;
using Service.Server.CQRS.Commands.Entities.StatisticsData;
using Service.Server.Old.Translators.StatisticsData;

namespace Service.Server.Old.Services.Entities.StatisticsData;

public class StatisticsDataCommandsService(
    IEventStore<StatisticsDataEvent> statisticsDataEventStore,
    IStatisticsDataCommandsToEventTranslator eventTranslator)
    : IStatisticsDataCommandsService
{
    public async Task ChangeTagSelection(ChangeTagSelectionCommand changeTagSelectionCommand)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(changeTagSelectionCommand));
    }

    public async Task ChangeProductivity(ChangeProductivityCommand changeProductivityCommand)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(changeProductivityCommand));
    }

    public async Task Create(CreateStatisticsDataCommand createStatisticsDataCommand)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(createStatisticsDataCommand));
    }
}