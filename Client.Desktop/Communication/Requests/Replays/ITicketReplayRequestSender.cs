using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels.Decorators.Replays;
using Proto.Requests.TicketReplay;

namespace Client.Desktop.Communication.Requests.Replays;

public interface ITicketReplayRequestSender
{
    Task<List<DocumentationReplay>> Send(ClientGetTicketReplaysByIdRequest request);
}