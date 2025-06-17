using Application.Contract.DTO;
using Application.Decorators;

namespace Application.Services.UseCase.Replays;

public interface IReplayRequestsService
{
    Task<TicketReplayDecorator> GetTicketReplayById(Guid ticketId);
}