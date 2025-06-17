using Application.Contract.CQRS.Commands.Entities.WorkDays;
using Application.Contract.CQRS.Requests.WorkDays;
using Application.Contract.DTO;
using Application.Services.Entities.WorkDays;
using Application.Services.UseCase;
using MediatR;

namespace Application.Handler.Requests.WorkDays;

public class GetSelectedWorkDayRequestHandler(
    IWorkDayRequestsService workDayRequestsService,
    IRunTimeSettings runTimeSettings,
    IWorkDayCommandsService workDayCommandsService) : IRequestHandler<GetSelectedWorkDayRequest, WorkDayDto>
{
    public async Task<WorkDayDto> Handle(GetSelectedWorkDayRequest request, CancellationToken cancellationToken)
    {
        var workDayDtos = await workDayRequestsService.GetAll();
        var currentWorkDayDto = workDayDtos.FirstOrDefault(w => w.Date.Date == runTimeSettings.SelectedDate.Date);

        if (currentWorkDayDto != null) return currentWorkDayDto;

        currentWorkDayDto = new WorkDayDto(Guid.NewGuid(), runTimeSettings.SelectedDate);

        await workDayCommandsService.Create(new CreateWorkDayCommand(currentWorkDayDto.WorkDayId,
            currentWorkDayDto.Date));

        return currentWorkDayDto;
    }
}