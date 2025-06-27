using Client.Desktop.Tracing.Communication.Tracing;
using Client.Desktop.Tracing.Tracing.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.Sprint.UseCase;

public class TicketAddedToSprintCollector(ITracingDataCommandSender commandSender) : ITicketAddedToSprintCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Add ticket to sprint requested for {attributes}";
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = ($"Received {notification}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task NoAggregateFound(Type originClassType, Guid traceId)
    {
        var log = ($"Aggregate not found id:{traceId}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = ($"Changed applied id:{traceId}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }
}