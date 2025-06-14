using System.Threading.Tasks;
using Contract.Decorators;
using Contract.DTO;

namespace Client.Desktop.Communication.RequiresChange;

public class AnalysisDataStub : IAnalysisDataService
{
    public Task<AnalysisByTagDecorator> GetAnalysisByTag(TagDto tagDto)
    {
        return Task.FromResult<AnalysisByTagDecorator>(null);
    }

    public Task<AnalysisByTicketDecorator> GetAnalysisByTicket(TicketDto ticketDto)
    {
        return Task.FromResult<AnalysisByTicketDecorator>(null);
    }

    public Task<AnalysisBySprintDecorator> GetAnalysisBySprint(SprintDto sprintDto)
    {
        return Task.FromResult<AnalysisBySprintDecorator>(null);
    }
}