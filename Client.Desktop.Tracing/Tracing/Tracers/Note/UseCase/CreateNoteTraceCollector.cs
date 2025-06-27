using Client.Desktop.Tracing.Communication.Tracing;
using Client.Desktop.Tracing.Tracing.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.Note.UseCase;

public class CreateNoteTraceCollector(ITracingDataCommandSender commandSender) : ICreateNoteTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Note creation requested {attributes}";
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, Guid.Empty,
            "toBeReplaced", originClassType, log));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = ($"Received aggregate {attributes}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = ($"Added aggregate with id:{traceId}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
    {
        var log = $"Exception occured {exception}";
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }
}