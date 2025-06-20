using Service.Server.CQRS.Requests.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Old.Handler.Requests.Sprints;

public class GetAllSprintsRequestHandler(ISprintRequestsService sprintRequestsService)
    : IRequestHandler<GetAllSprintsRequest, List<SprintDto>>
{
    public async Task<List<SprintDto>> Handle(GetAllSprintsRequest request, CancellationToken cancellationToken)
    {
        return await sprintRequestsService.GetAll();
    }
}