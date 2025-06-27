namespace Client.Desktop.Tracing.Tracing.Tracers.Ticket.UseCase;

public class AddTicketToCurrentSprintTraceCollector()
    : IAddTicketToCurrentSprintTraceCollector
{
    public void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Add ticket to sprint requested for {attributes}";
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }
}