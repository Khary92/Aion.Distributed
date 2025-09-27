using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public class CreateTicketUseCaseCollector(ITracingDataSender sender) : ICreateTicketUseCaseCollector
{
    private const SortingType Sorting = SortingType.Ticket;
    private const UseCaseMeta UseCase = UseCaseMeta.CreateTicket;

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {GetName(notification)}:{notification}";

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

    private static string GetName(object @object)
    {
        var commandType = @object.GetType();
        return commandType.Name;
    }
}