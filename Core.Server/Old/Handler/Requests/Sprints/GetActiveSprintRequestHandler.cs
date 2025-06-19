using Service.Server.CQRS.Requests.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Old.Handler.Requests.Sprints;

public class GetActiveSprintRequestHandler(ISprintRequestsService sprintRequestsService) :
    IRequestHandler<GetActiveSprintRequest, SprintDto?>
{
    public async Task<SprintDto?> Handle(GetActiveSprintRequest request, CancellationToken cancellationToken)
    {
        return (await sprintRequestsService.GetAll()).FirstOrDefault(s => s.IsActive);
    }
}