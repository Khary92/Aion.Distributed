using System.Threading.Tasks;
using Client.Desktop.DataModels.Decorators;
using Proto.Requests.AnalysisData;

namespace Client.Desktop.Communication.Requests.Analysis;

public interface IAnalysisRequestSender
{
    Task<AnalysisBySprintDecorator> Send(GetSprintAnalysisById request);
    Task<AnalysisByTicketDecorator> Send(GetTicketAnalysisById request);
    Task<AnalysisByTagDecorator> Send(GetTagAnalysisById request);
}