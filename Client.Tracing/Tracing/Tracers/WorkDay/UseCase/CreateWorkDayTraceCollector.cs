using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.WorkDay.UseCase;

public class CreateWorkDayTraceCollector(ITracingDataSender sender) : ICreateWorkDayTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = "Create WorkDay requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.WorkDay,
            UseCaseMeta.CreateWorkDay,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingCommand(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.WorkDay,
            UseCaseMeta.CommandSent,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Received aggregate {attributes}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.WorkDay,
            UseCaseMeta.CreateWorkDay,
            LoggingMeta.AggregateReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = $"Added aggregate with id:{traceId}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.WorkDay,
            UseCaseMeta.CreateWorkDay,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}