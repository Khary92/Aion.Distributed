using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Core.Server.Tracing.Tracing.Tracers.Tag.UseCase;

public class CreateTagTraceCollector(ITracingDataCommandSender commandSender) : ICreateTagTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Create Tag requested for {attributes}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.CommandSent,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = ($"Received aggregate {attributes}");
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.AggregateReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = ($"Added aggregate with id:{traceId}");
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}