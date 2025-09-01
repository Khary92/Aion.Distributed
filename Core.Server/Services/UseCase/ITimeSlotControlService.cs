namespace Core.Server.Services.UseCase;

public interface ITimeSlotControlService
{
    Task Create(Guid ticketId, DateTimeOffset date, Guid traceId);
}