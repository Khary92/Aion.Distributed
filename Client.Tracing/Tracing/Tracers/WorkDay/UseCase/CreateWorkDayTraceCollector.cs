using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.WorkDay.UseCase;

public class CreateWorkDayTraceCollector(ITracingDataSender sender) : ICreateWorkDayTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Create WorkDay requested";

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
            UseCaseMeta.CreateWorkDay,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ActionAborted(Type originClassType, Guid traceId)
    {
        const string log = "Action aborted because work day already exists";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.WorkDay,
            UseCaseMeta.CreateWorkDay,
            LoggingMeta.ActionAborted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {GetName(notification)}:{notification}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.WorkDay,
            UseCaseMeta.CreateWorkDay,
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
            SortingType.WorkDay,
            UseCaseMeta.CreateWorkDay,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    private static string GetName(object @object)
    {
        var commandType = @object.GetType();
        return commandType.Name;
    }
}