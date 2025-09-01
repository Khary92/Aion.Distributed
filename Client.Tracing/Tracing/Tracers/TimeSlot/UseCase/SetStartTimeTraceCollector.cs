using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;

public class SetStartTimeTraceCollector(ITracingDataSender sender) : ISetStartTimeTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Requested pushing start time slot data after failed shutdown";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimeSlot,
            UseCaseMeta.SetStartTime,
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
            SortingType.TimeSlot,
            UseCaseMeta.SetStartTime,
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
            SortingType.TimeSlot,
            UseCaseMeta.SetStartTime,
            LoggingMeta.ActionAborted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}