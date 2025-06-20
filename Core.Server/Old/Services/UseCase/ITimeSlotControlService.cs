using Service.Server.CQRS.Commands.UseCase;

namespace Service.Server.Old.Services.UseCase;

public interface ITimeSlotControlService
{
    Task Create(Guid ticketId);
    Task Load(Guid timeSlotId);
}