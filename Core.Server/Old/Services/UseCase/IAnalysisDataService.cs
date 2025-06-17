using Application.Contract.DTO;
using Application.Decorators;

namespace Application.Services.UseCase;

public interface IAnalysisDataService
{
    Task<AnalysisByTagDecorator> GetAnalysisByTag(TagDto tagDto);
    Task<AnalysisByTicketDecorator> GetAnalysisByTicket(TicketDto ticketDto);
    Task<AnalysisBySprintDecorator> GetAnalysisBySprint(SprintDto sprintDto);
}