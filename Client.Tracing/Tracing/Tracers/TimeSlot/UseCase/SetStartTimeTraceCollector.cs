using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;

public class SetStartTimeTraceCollector(ITracingDataSender sender) : ISetStartTimeTraceCollector
{
    private const SortingType Sorting = SortingType.TimeSlot;
    private const UseCaseMeta UseCase = UseCaseMeta.SetStartTime;

    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Requested pushing start time slot data after failed shutdown";

        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingCommand(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CacheIsEmpty(Type originClassType, Guid traceId)
    {
        const string log = "Aborted because cache is empty";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.ActionAborted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}