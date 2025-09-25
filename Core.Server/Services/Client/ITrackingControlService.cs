namespace Core.Server.Services.Client;

public interface ITrackingControlService
{
    Task Create(Guid ticketId, DateTimeOffset date, Guid traceId);
}