using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.Client.UseCase;

public class CreateTrackingControlCollector(ITracingDataSender sender) : ICreateTrackingControlCollector
{
    private const SortingType Sorting = SortingType.Client;
    private const UseCaseMeta UseCase = UseCaseMeta.CreateTrackingControl;

    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = "Creation of tracking control requested";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingCommand(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sending command {command}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {notification}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = $"Added aggregate with id:{traceId}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
    {
        var log = $"Exception occured {exception}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.ExceptionOccured,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}