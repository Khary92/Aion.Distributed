using Service.Server.CQRS.Requests.WorkDays;
using Service.Server.Old.Services.Entities.WorkDays;

namespace Service.Server.Old.Handler.Requests.WorkDays;

public class GetWorkDayByDateRequestHandler(IWorkDayRequestsService workDayRequestsService)
    : IRequestHandler<GetWorkDayByDateRequest, WorkDayDto?>
{
    public async Task<WorkDayDto?> Handle(GetWorkDayByDateRequest request, CancellationToken cancellationToken)
    {
        var workDayDtos = await workDayRequestsService.GetAll();
        return workDayDtos.FirstOrDefault(workDay => workDay.Date.Date == request.Date.Date);
    }
}