namespace Service.Server.Old.Services.UseCase.Replays;

public interface IReplayRequestsService
{
    Task<TicketReplayDecorator> GetTicketReplayById(Guid ticketId);
}