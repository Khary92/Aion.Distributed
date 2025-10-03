using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Client;
using Client.Desktop.Communication.Requests.Client.Records;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockClientRequestSender(MockDataService mockDataService) : IClientRequestSender
{
    public Task<List<ClientGetTrackingControlResponse>> Send(ClientGetTrackingControlDataRequest request)
    {
        return Task.FromResult(mockDataService.GetInitialClientGetTrackingControlResponses());
    }
}