using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Core.Server.Translators.Commands.StatisticsData;
using Domain.Events.StatisticsData;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.StatisticsData;

public class StatisticsDataCommandsService(
    StatisticsDataNotificationService statisticsDataNotificationService,
    IEventStore<StatisticsDataEvent> statisticsDataEventStore,
    IStatisticsDataCommandsToEventTranslator eventTranslator)
    : IStatisticsDataCommandsService
{
    public async Task ChangeTagSelection(ChangeTagSelectionCommand changeTagSelectionCommand)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(changeTagSelectionCommand));
        await statisticsDataNotificationService.SendNotificationAsync(changeTagSelectionCommand.ToNotification());
    }

    public async Task ChangeProductivity(ChangeProductivityCommand changeProductivityCommand)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(changeProductivityCommand));
        await statisticsDataNotificationService.SendNotificationAsync(changeProductivityCommand.ToNotification());
    }

    public async Task Create(CreateStatisticsDataCommand createStatisticsDataCommand)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(createStatisticsDataCommand));
        await statisticsDataNotificationService.SendNotificationAsync(createStatisticsDataCommand.ToNotification());
    }
}