using Core.Server.Communication.Tracing;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Core.Server.Tracing.Tracing.Tracers.Client.UseCase;

public class CreateTrackingControlCollector(ITracingDataSender sender) : ICreateTrackingControlCollector
{
    public async Task CommandReceived(Type originClassType, Guid traceId, object protoCommand)
    {
        var log = $"Command received {GetName(protoCommand)}:{protoCommand}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Client,
            UseCaseMeta.CreateTrackingControl,
            LoggingMeta.CommandReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task EventPersisted(Type originClassType, Guid traceId, object @event)
    {
        var log = $"Event persisted {@event}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Client,
            UseCaseMeta.CreateTrackingControl,
            LoggingMeta.EventPersisted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingNotification(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Notification sent {GetName(notification)}:{notification}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Client,
            UseCaseMeta.CreateTrackingControl,
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