using Application.Contract.CQRS.Requests.Replays;
using Application.Contract.DTO;
using Application.Services.UseCase.Replays;
using MediatR;

namespace Application.Handler.Requests.Replays;

public class GetTicketReplayByIdRequestHandler(IReplayRequestsService replayRequestsService)
    : IRequestHandler<GetTicketReplayByIdRequest, TicketReplayDecorator>
{
    public async Task<TicketReplayDecorator> Handle(GetTicketReplayByIdRequest request,
        CancellationToken cancellationToken)
    {
        return await replayRequestsService.GetTicketReplayById(request.TicketId);
    }
}