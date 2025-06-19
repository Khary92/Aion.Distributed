using Service.Server.CQRS.Requests.WorkDays;
using Service.Server.Old.Services.Entities.WorkDays;

namespace Service.Server.Old.Handler.Requests.WorkDays;

public class GetAllWorkDaysRequestHandler(
    IWorkDayRequestsService workDayRequestsService) : IRequestHandler<GetAllWorkDaysRequest, List<WorkDayDto>>
{
    public async Task<List<WorkDayDto>> Handle(GetAllWorkDaysRequest request, CancellationToken cancellationToken)
    {
        return await workDayRequestsService.GetAll();
    }
}