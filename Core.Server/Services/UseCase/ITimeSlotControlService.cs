namespace Service.Server.Services.UseCase;

public interface ITimeSlotControlService
{
    Task Create(Guid ticketId);
    Task Load(Guid timeSlotId);
}