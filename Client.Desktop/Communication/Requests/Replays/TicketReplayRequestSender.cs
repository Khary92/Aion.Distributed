using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels.Decorators.Replays;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.TicketReplay;

namespace Client.Desktop.Communication.Requests.Replays;

public class TicketReplayRequestSender : ITicketReplayRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TicketReplayRequestService.TicketReplayRequestServiceClient _client = new(Channel);

    public async Task<List<DocumentationReplay>> Send(ClientGetTicketReplaysByIdRequest request)
    {
        var response = await _client.GetReplayDataAsync(request.ToProto());
        return response.ToReplayList();
    }
}