using Domain.Events.StatisticsData;
using Domain.Interfaces;
using Service.Server.Communication.StatisticsData;
using Service.Server.CQRS.Commands.Entities.StatisticsData;
using Service.Server.Old.Translators.StatisticsData;

namespace Service.Server.Old.Services.Entities.StatisticsData;

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