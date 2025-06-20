using Domain.Entities;
using Proto.DTO.AnalysisBySprint;
using Proto.DTO.AnalysisByTag;
using Proto.DTO.AnalysisByTicket;

namespace Service.Server.Services.UseCase;

public interface IAnalysisDataService
{
    Task<AnalysisByTagProto> GetAnalysisByTag(Tag tag);
    Task<AnalysisByTicketProto> GetAnalysisByTicket(Ticket ticket);
    Task<AnalysisBySprintProto> GetAnalysisBySprint(Sprint sprint);
}