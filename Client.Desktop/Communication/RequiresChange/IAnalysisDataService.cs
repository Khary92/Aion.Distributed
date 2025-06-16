using System.Threading.Tasks;
using Client.Desktop.Decorators;
using Contract.Decorators;
using Contract.DTO;

namespace Client.Desktop.Communication.RequiresChange;

public interface IAnalysisDataService
{
    Task<AnalysisByTagDecorator> GetAnalysisByTag(TagDto tagDto);
    Task<AnalysisByTicketDecorator> GetAnalysisByTicket(TicketDto ticketDto);
    Task<AnalysisBySprintDecorator> GetAnalysisBySprint(SprintDto sprintDto);
}