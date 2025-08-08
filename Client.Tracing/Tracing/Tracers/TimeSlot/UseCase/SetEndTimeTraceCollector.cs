using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;

public class SetEndTimeTraceCollector(ITracingDataCommandSender commandSender) : ISetEndTimeTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = $"Requested pushing end time slot data after failed shutdown";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimeSlot,
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
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimeSlot,
            UseCaseMeta.SetEndTime,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CacheIsEmpty(Type originClassType, Guid traceId)
    {
        var log = $"Aborted because cache is empty";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimeSlot,
            UseCaseMeta.SetEndTime,
            LoggingMeta.ActionAborted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    private static string GetName(object @object)
    {
        var commandType = @object.GetType();
        return commandType.Name;
    }
}