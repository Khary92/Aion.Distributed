using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.DataModels.Decorators;

namespace Client.Desktop.Communication.Requests.Analysis;

public interface IAnalysisRequestSender
{
    Task<AnalysisBySprintDecorator> Send(ClientGetSprintAnalysisById request);
    Task<AnalysisByTicketDecorator> Send(ClientGetTicketAnalysisById request);
    Task<AnalysisByTagDecorator> Send(ClientGetTagAnalysisById request);
}