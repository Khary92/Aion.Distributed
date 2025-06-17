using Application.Contract.CQRS.Commands.Entities.StatisticsData;
using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Application.Contract.CQRS.Commands.UseCase.Commands;
using Application.Contract.Notifications.UseCase;
using Application.Services.Entities.StatisticsData;
using Application.Services.Entities.TimeSlots;
using Application.Services.Entities.WorkDays;
using Application.Services.UseCase;
using MediatR;

namespace Application.Handler.Commands.UseCase;

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