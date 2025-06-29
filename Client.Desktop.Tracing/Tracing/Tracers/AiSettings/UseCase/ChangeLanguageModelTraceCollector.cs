using Client.Desktop.Proto.Tracing.Enums;
using Client.Desktop.Tracing.Communication.Tracing;

namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

public class ChangeLanguageModelTraceCollector(ITracingDataCommandSender commandSender)
    : IChangeLanguageModelTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, (string, string) attribute)
    {
        var log = ($"Change LanguageModel requested for {attribute.Item1}:{attribute.Item2}");

        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.AiSettings,
            UseCaseMeta.ChangeLanguageModel,
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
            TraceSinkId.AiSettings,
            UseCaseMeta.ChangeLanguageModel,
            LoggingMeta.CommandSent,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));}

    public async Task PropertyNotChanged(Type originClassType, Guid traceId, (string, string) attribute)
    {
        var log = ($"Request aborted {attribute.Item1}:{attribute.Item2}");
        
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.AiSettings,
            UseCaseMeta.ChangeLanguageModel,
            LoggingMeta.PropertyNotChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));}
}