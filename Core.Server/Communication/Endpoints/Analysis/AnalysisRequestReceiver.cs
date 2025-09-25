using Core.Server.Services.Client;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Services.Entities.Tags;
using Core.Server.Services.Entities.Tickets;
using Grpc.Core;
using Proto.DTO.AnalysisBySprint;
using Proto.DTO.AnalysisByTag;
using Proto.DTO.AnalysisByTicket;
using Proto.Requests.AnalysisData;

namespace Core.Server.Communication.Endpoints.Analysis;

public class AnalysisRequestReceiver(
    IAnalysisDataService analysisDataService,
    ISprintRequestsService sprintRequestsService,
    ITagRequestsService tagRequestsService,
    ITicketRequestsService ticketRequestsService) : AnalysisRequestService.AnalysisRequestServiceBase
{
    public override async Task<AnalysisBySprintProto> GetSprintAnalysis(GetSprintAnalysisById request,
        ServerCallContext context)
    {
        var selectedSprint = await sprintRequestsService.GetById(Guid.Parse(request.SprintId));

        return await analysisDataService.GetAnalysisBySprint(selectedSprint!);
    }

    public override async Task<AnalysisByTagProto> GetTagAnalysis(GetTagAnalysisById request, ServerCallContext context)
    {
        var selectedTag = await tagRequestsService.GetTagById(Guid.Parse(request.TagId));

        return await analysisDataService.GetAnalysisByTag(selectedTag);
    }

    public override async Task<AnalysisByTicketProto> GetTicketAnalysis(GetTicketAnalysisById request,
        ServerCallContext context)
    {
        var selectedTicket = await ticketRequestsService.GetTicketById(Guid.Parse(request.TicketId));

        return await analysisDataService.GetAnalysisByTicket(selectedTicket!);
    }
}