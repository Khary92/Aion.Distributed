using Client.Desktop.Tracing.Communication.Tracing;
using Client.Desktop.Tracing.Tracing.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

public class ChangeLanguageModelTraceCollector(ITracingDataCommandSender commandSender)
    : IChangeLanguageModelTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, (string, string) attribute)
    {
        var log = ($"Change LanguageModel requested for {attribute.Item1}:{attribute.Item2}");

        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");

        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId,
            "toBeReplaced", originClassType, log));
    }

    public async Task PropertyNotChanged(Type originClassType, Guid traceId, (string, string) attribute)
    {
        var log = ($"Request aborted {attribute.Item1}:{attribute.Item2}");
        await commandSender.Send(new TraceDataCommand(DateTimeOffset.Now, LoggingMeta.PropertyNotChanged, traceId,
            "toBeReplaced", originClassType, log));
    }
}