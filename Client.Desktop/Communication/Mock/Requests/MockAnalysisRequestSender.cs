using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.DataModels.Decorators;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockAnalysisRequestSender(MockDataService mockDataService) : IAnalysisRequestSender
{
    public Task<AnalysisBySprintDecorator> Send(ClientGetSprintAnalysisById request)
    {
        throw new System.NotImplementedException();
    }

    public Task<AnalysisByTicketDecorator> Send(ClientGetTicketAnalysisById request)
    {
        throw new System.NotImplementedException();
    }

    public Task<AnalysisByTagDecorator> Send(ClientGetTagAnalysisById request)
    {
        throw new System.NotImplementedException();
    }
}