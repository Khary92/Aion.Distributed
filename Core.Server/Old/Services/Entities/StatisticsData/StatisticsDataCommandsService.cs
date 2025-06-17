using Application.Contract.CQRS.Commands.Entities.StatisticsData;
using Application.Translators.StatisticsData;
using Domain.Events.StatisticsData;
using Domain.Interfaces;

namespace Application.Services.Entities.StatisticsData;

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