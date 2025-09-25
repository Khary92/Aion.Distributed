using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Client.Records;
using Client.Proto;
using Grpc.Net.Client;
using Proto.Requests.Client;

namespace Client.Desktop.Communication.Requests.Client;

public class ClientRequestSender : IClientRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly ClientRequestService.ClientRequestServiceClient _client = new(Channel);

    public async Task<List<ClientGetTrackingControlResponse>> Send(ClientGetTrackingControlDataRequest request)
    {
        var timeSlotControlDatalist = await _client.GetTrackingControlDataByDateAsync(request.ToProto());
        return timeSlotControlDatalist.ToResponseDataList();
    }
}