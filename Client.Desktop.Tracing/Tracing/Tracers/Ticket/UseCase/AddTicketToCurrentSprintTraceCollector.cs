using Client.Desktop.Tracing.Communication.Tracing;
using Client.Desktop.Tracing.Tracing.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.Ticket.UseCase;

public class AddTicketToCurrentSprintTraceCollector(ITracingDataCommandSender commandSender)
    : IAddTicketToCurrentSprintTraceCollector
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
}