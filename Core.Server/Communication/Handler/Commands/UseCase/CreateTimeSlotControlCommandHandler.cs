using Service.Server.CQRS.Commands.Entities.StatisticsData;
using Service.Server.CQRS.Commands.Entities.TimeSlots;
using Service.Server.CQRS.Commands.UseCase.Commands;
using Service.Server.Old.Services.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.WorkDays;
using Service.Server.Old.Services.UseCase;

namespace Service.Server.Old.Handler.Commands.UseCase;

public class CreateTimeSlotControlCommandHandler(
    IMediator mediator,
    ITimeSlotCommandsService timeSlotCommandsService,
    IStatisticsDataCommandsService statisticsDataCommandsService,
    IWorkDayRequestsService workDayRequestsService,
    IRunTimeSettings runTimeSettings) : IRequestHandler<CreateTimeSlotControlCommand, Unit>
{
    public async Task<Unit> Handle(CreateTimeSlotControlCommand command,
        CancellationToken cancellationToken)
    {
        var currentWorkDayId = (await workDayRequestsService.GetAll())
            .First(wd => wd.Date.Date == runTimeSettings.SelectedDate.Date).WorkDayId;

        var newTimeSlotId = Guid.NewGuid();
        await timeSlotCommandsService.Create(new CreateTimeSlotCommand(newTimeSlotId, command.TicketId,
            currentWorkDayId, DateTimeOffset.Now, DateTimeOffset.Now, false));

        var newStatisticsDataId = Guid.NewGuid();
        await statisticsDataCommandsService.Create(new CreateStatisticsDataCommand(newStatisticsDataId,
            true, false, false, [], newTimeSlotId));

        await mediator.Publish(new TimeSlotControlCreatedNotification(command.ViewId, newTimeSlotId, command.TicketId),
            cancellationToken);

        return Unit.Value;
    }
}