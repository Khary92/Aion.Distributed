using Client.Desktop.Proto.Tracing.Enums;
using Client.Desktop.Tracing.Communication.Tracing;

namespace Client.Desktop.Tracing.Tracing.Tracers.Sprint.UseCase;

public class CreateSprintTraceCollector(ITracingDataCommandSender commandSender) : ICreateSprintTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Create Sprint requested for {attributes}";
    
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.CreateSprint,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.CreateSprint,
            LoggingMeta.CommandSent,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = ($"Received aggregate {attributes}");
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.CreateSprint,
            LoggingMeta.AggregateReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = ($"Added aggregate with id:{traceId}");
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.CreateSprint,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}