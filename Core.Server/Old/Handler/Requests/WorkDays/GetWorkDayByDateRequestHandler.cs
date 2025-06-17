using Application.Contract.CQRS.Requests.WorkDays;
using Application.Contract.DTO;
using Application.Services.Entities.WorkDays;
using MediatR;

namespace Application.Handler.Requests.WorkDays;

public class GetWorkDayByDateRequestHandler(IWorkDayRequestsService workDayRequestsService)
    : IRequestHandler<GetWorkDayByDateRequest, WorkDayDto?>
{
    public async Task<WorkDayDto?> Handle(GetWorkDayByDateRequest request, CancellationToken cancellationToken)
    {
        var workDayDtos = await workDayRequestsService.GetAll();
        return workDayDtos.FirstOrDefault(workDay => workDay.Date.Date == request.Date.Date);
    }
}