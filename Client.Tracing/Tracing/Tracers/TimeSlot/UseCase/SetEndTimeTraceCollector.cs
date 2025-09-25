using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;

public class SetEndTimeTraceCollector(ITracingDataSender sender) : ISetEndTimeTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Requested pushing end time slot data after failed shutdown";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimeSlot,
            UseCaseMeta.SetEndTime,
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
            UseCaseMeta.SetEndTime,
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
            UseCaseMeta.SetEndTime,
            LoggingMeta.ActionAborted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task FlushingCacheFailed(Type originClassType, Guid traceId, string filePath)
    {
        var log = "The data was persisted, but the file could not be deleted " + filePath;
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimeSlot,
            UseCaseMeta.SetEndTime,
            LoggingMeta.ExceptionOccured,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}