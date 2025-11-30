using Core.Server.Services.Entities.Tickets;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.DTO.TicketReplay;
using Proto.Requests.TicketReplay;

namespace Core.Server.Communication.Endpoints.TicketReplay;

[Authorize]
public class TicketReplayRequestReceiver(ITicketRequestsService ticketRequestsService)
    : TicketReplayRequestService.TicketReplayRequestServiceBase
{
    public override async Task<GetReplayResponseProto> GetReplayData(GetTicketReplaysByIdRequestProto request,
        ServerCallContext context)
    {
        var documentation = await ticketRequestsService.GetDocumentationByTicketId(Guid.Parse(request.TicketId));
        var getReplayResponseProto = new GetReplayResponseProto
        {
            TicketReplays =
            {
                documentation.ConvertAll(d => new TicketReplayProto
                {
                    DocumentationEntry = d
                })
            }
        };

        return getReplayResponseProto;
    }
}