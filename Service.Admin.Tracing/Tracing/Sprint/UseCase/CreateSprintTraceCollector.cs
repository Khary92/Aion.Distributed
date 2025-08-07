using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Sprint.UseCase;

public class CreateSprintTraceCollector(ITracingDataCommandSender commandSender) : ICreateSprintTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = $"Create Sprint requested";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.CreateSprint,
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
            TraceSinkId.Sprint,
            UseCaseMeta.CreateSprint,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Received aggregate {attributes}";
        await commandSender.Send(new ServiceTraceDataCommand(
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
        var log = $"Added aggregate with id:{traceId}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.CreateSprint,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}