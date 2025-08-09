using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public class AddTicketToCurrentSprintTraceCollector(ITracingDataCommandSender commandSender)
    : IAddTicketToCurrentSprintTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Add ticket to sprint requested for {attributes}";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}