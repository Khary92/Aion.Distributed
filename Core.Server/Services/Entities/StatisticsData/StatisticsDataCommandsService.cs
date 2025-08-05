using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.StatisticsData;
using Domain.Events.StatisticsData;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.StatisticsData;

public class StatisticsDataCommandsService(
    StatisticsDataNotificationService statisticsDataNotificationService,
    IEventStore<StatisticsDataEvent> statisticsDataEventStore,
    IStatisticsDataCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer)
    : IStatisticsDataCommandsService
{
    public async Task ChangeTagSelection(ChangeTagSelectionCommand command)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var statistics = command.ToNotification();
        await tracer.Statistics.ChangeTagSelection.EventPersisted(GetType(), command.TraceId,
            statistics.ChangeTagSelection);

        await tracer.Statistics.ChangeTagSelection.SendingNotification(GetType(), command.TraceId,
            statistics.ChangeTagSelection);
        await statisticsDataNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task ChangeProductivity(ChangeProductivityCommand command)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var statistics = command.ToNotification();
        await tracer.Statistics.ChangeProductivity.EventPersisted(GetType(), command.TraceId,
            statistics.ChangeProductivity);

        await tracer.Statistics.ChangeProductivity.SendingNotification(GetType(), command.TraceId,
            statistics.ChangeProductivity);
        await statisticsDataNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateStatisticsDataCommand command)
    {
        await statisticsDataEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var statistics = command.ToNotification();
        await tracer.Statistics.Create.EventPersisted(GetType(), command.TraceId,
            statistics.StatisticsDataCreated);

        await tracer.Statistics.Create.SendingNotification(GetType(), command.TraceId,
            statistics.StatisticsDataCreated);
        await statisticsDataNotificationService.SendNotificationAsync(command.ToNotification());
    }
}