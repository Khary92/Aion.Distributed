using Service.Server.CQRS.Requests.Sprints;
using Service.Server.Old.Services.Entities.Sprints;

namespace Service.Server.Communication.Sprint.Handlers;

public class GetActiveSprintRequestHandler(ISprintRequestsService sprintRequestsService)
{
    public async Task<Domain.Entities.Sprint?> Handle(GetActiveSprintRequest request)
    {
        return (await sprintRequestsService.GetAll()).FirstOrDefault(s => s.IsActive);
    }
}