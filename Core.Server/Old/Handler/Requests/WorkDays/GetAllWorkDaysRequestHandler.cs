using Application.Contract.CQRS.Requests.WorkDays;
using Application.Contract.DTO;
using Application.Services.Entities.WorkDays;
using MediatR;

namespace Application.Handler.Requests.WorkDays;

public class GetAllWorkDaysRequestHandler(
    IWorkDayRequestsService workDayRequestsService) : IRequestHandler<GetAllWorkDaysRequest, List<WorkDayDto>>
{
    public async Task<List<WorkDayDto>> Handle(GetAllWorkDaysRequest request, CancellationToken cancellationToken)
    {
        return await workDayRequestsService.GetAll();
    }
}