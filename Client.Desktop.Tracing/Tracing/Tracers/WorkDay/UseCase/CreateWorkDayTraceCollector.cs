using Core.Server.Tracing.Communication.Tracing;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.WorkDay.UseCase;

public class CreateWorkDayTraceCollector(ITracingDataCommandSender commandSender) : ICreateWorkDayTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Create WorkDay requested for {attributes}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.WorkDay,
            UseCaseMeta.CreateWorkDay,
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
            TraceSinkId.WorkDay,
            UseCaseMeta.CommandSent,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = ($"Received aggregate {attributes}");

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.WorkDay,
            UseCaseMeta.CreateWorkDay,
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
            TraceSinkId.WorkDay,
            UseCaseMeta.CreateWorkDay,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}