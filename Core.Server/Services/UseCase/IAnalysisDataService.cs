namespace Service.Server.Old.Services.UseCase;

public interface IAnalysisDataService
{
    Task<AnalysisByTagDecorator> GetAnalysisByTag(TagDto tagDto);
    Task<AnalysisByTicketDecorator> GetAnalysisByTicket(TicketDto ticketDto);
    Task<AnalysisBySprintDecorator> GetAnalysisBySprint(SprintDto sprintDto);
}