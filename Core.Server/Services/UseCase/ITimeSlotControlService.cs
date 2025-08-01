namespace Core.Server.Services.UseCase;

public interface ITimeSlotControlService
{
    Task Create(Guid ticketId, Guid traceId);
    Task Load(Guid timeSlotId, Guid traceId);
}