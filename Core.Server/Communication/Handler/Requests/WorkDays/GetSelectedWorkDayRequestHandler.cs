using Service.Server.CQRS.Commands.Entities.WorkDays;
using Service.Server.CQRS.Requests.WorkDays;
using Service.Server.Old.Services.Entities.WorkDays;
using Service.Server.Old.Services.UseCase;

namespace Service.Server.Old.Handler.Requests.WorkDays;

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