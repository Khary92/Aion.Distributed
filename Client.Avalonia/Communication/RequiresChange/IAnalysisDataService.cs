using System.Threading.Tasks;
using Contract.Decorators;
using Contract.DTO;

namespace Client.Avalonia.Communication.RequiresChange;

public interface IAnalysisDataService
{
    Task<AnalysisByTagDecorator> GetAnalysisByTag(TagDto tagDto);
    Task<AnalysisByTicketDecorator> GetAnalysisByTicket(TicketDto ticketDto);
    Task<AnalysisBySprintDecorator> GetAnalysisBySprint(SprintDto sprintDto);
}