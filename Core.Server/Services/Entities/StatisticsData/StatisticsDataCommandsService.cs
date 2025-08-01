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
    public async Task ChangeTagSelection(ChangeTagSelectionCommand command)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await statisticsDataNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeProductivity(ChangeProductivityCommand command)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await statisticsDataNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateStatisticsDataCommand command)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        await statisticsDataNotificationService.SendNotificationAsync(command.ToNotification());
    }
}