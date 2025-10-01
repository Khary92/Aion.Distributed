using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Client.Records;
using Grpc.Net.Client;
using Proto.Requests.Client;

namespace Client.Desktop.Communication.Requests.Client;

public class ClientRequestSender : IClientRequestSender
{
    private readonly ClientRequestService.ClientRequestServiceClient _client;

    public ClientRequestSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new ClientRequestService.ClientRequestServiceClient(channel);
    }

    public async Task<List<ClientGetTrackingControlResponse>> Send(ClientGetTrackingControlDataRequest request)
    {
        var timeSlotControlDatalist = await _client.GetTrackingControlDataByDateAsync(request.ToProto());
        return timeSlotControlDatalist.ToResponseDataList();
    }
}