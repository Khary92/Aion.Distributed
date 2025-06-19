using Service.Server.CQRS.Requests.Replays;
using Service.Server.Old.Services.UseCase.Replays;

namespace Service.Server.Old.Handler.Requests.Replays;

public class GetTicketReplayByIdRequestHandler(IReplayRequestsService replayRequestsService)
    : IRequestHandler<GetTicketReplayByIdRequest, TicketReplayDecorator>
{
    public async Task<TicketReplayDecorator> Handle(GetTicketReplayByIdRequest request,
        CancellationToken cancellationToken)
    {
        return await replayRequestsService.GetTicketReplayById(request.TicketId);
    }
}