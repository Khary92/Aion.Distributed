using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Core.Server.Tracing.Tracing.Tracers.Statistics.UseCase;

public class ChangeProductivityTraceCollector(ITracingDataCommandSender commandSender) : IChangeProductivityTraceCollector
{
    public async Task CommandReceived(Type originClassType, Guid traceId, object protoCommand)
    {
        var log = $"Command received {GetName(protoCommand)}:{protoCommand}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.StatisticsData,
            UseCaseMeta.ChangeProductivity,
            LoggingMeta.CommandReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task EventPersisted(Type originClassType, Guid traceId, object @event)
    {
        var log = $"Event persisted {@event}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.StatisticsData,
            UseCaseMeta.ChangeProductivity,
            LoggingMeta.EventPersisted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingNotification(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Notification sent {GetName(notification)}:{notification}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.StatisticsData,
            UseCaseMeta.ChangeProductivity,
            LoggingMeta.SendingNotification,
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