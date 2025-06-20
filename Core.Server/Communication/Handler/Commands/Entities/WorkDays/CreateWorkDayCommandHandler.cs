using Service.Server.CQRS.Commands.Entities.WorkDays;
using Service.Server.Old.Services.Entities.WorkDays;

namespace Service.Server.Old.Handler.Commands.Entities.WorkDays;

public class CreateWorkDayCommandHandler(
    IWorkDayCommandsService workDayCommandsService,
    IWorkDayRequestsService workDayRequestsService)
    : IRequestHandler<CreateWorkDayCommand, Unit>
{
    public async Task<Unit> Handle(CreateWorkDayCommand command, CancellationToken cancellationToken)
    {
        var workDays = await workDayRequestsService.GetAll();

        if (workDays.Any(wd => wd.Date.Date == command.Date.Date)) return Unit.Value;

        await workDayCommandsService.Create(command);
        return Unit.Value;
    }
}