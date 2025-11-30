using Core.Server.Services.Entities.Sprints;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.DTO.Sprint;
using Proto.Requests.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint;

[Authorize]
public class SprintRequestReceiver(
    ISprintRequestsService sprintRequestsService) : SprintProtoRequestService.SprintProtoRequestServiceBase
{
    public override async Task<SprintProto?> GetActiveSprint(GetActiveSprintRequestProto request,
        ServerCallContext context)
    {
        var sprint = await sprintRequestsService.GetActiveSprint();
        return sprint == null ? new SprintProto() : sprint.ToProto();
    }

    public override async Task<SprintListProto> GetAllSprints(GetAllSprintsRequestProto request,
        ServerCallContext context)
    {
        var sprints = await sprintRequestsService.GetAll();
        return sprints.ToProtoList();
    }
}