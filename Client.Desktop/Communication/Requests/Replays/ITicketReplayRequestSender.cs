using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Replays;
using Proto.Requests.TicketReplay;

namespace Client.Desktop.Communication.Requests.Replays;

public interface ITicketReplayRequestSender
{
    Task<List<DocumentationReplayDto>> Send(GetTicketReplaysByIdRequestProto request);
}