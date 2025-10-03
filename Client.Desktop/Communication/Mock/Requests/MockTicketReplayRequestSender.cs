using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.DataModels.Decorators.Replays;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockTicketReplayRequestSender(MockDataService mockDataService) : ITicketReplayRequestSender
{
    public Task<List<DocumentationReplay>> Send(ClientGetTicketReplaysByIdRequest request)
    {
        return Task.FromResult(mockDataService.DocumentationReplays);
    }
}