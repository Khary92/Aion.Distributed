namespace Core.Server.Tracing.Tracing.Tracers.Sprint.UseCase;

public interface ITicketAddedToSprintCollector
{
    Task CommandReceived(Type originClassType, Guid traceId, object protoCommand);
    Task EventPersisted(Type originClassType, Guid traceId, object @event);
    Task SendingNotification(Type originClassType, Guid traceId, object notification);
    Task TicketNotFound(Type originClassType, Guid traceId);
    Task NoActiveSprint(Type originClassType, Guid traceId);
    Task AddedSprintIdToTicket(Type originClassType, Guid traceId, Guid activeSprintSprintId);
    Task TicketIsAlreadyInSprint(Type originClassType, Guid traceId);
}