using Client.Desktop.Tracing.Communication.Tracing;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

public class ChangePromptTraceCollector(ITracingDataCommandSender commandSender) : IChangePromptTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, (string, string) attributes)
    {
        var log = ($"Change Prompt requested for {attributes.Item1}:{attributes.Item2}");
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.AiSettings,
            UseCaseMeta.ChangePrompt,
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
            TraceSinkId.AiSettings,
            UseCaseMeta.ChangePrompt,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task PropertyNotChanged(Type originClassType, Guid traceId, (string, string) property)
    {
        var log = ($"Request aborted {property.Item1}:{property.Item2}");

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.AiSettings,
            UseCaseMeta.ChangePrompt,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}