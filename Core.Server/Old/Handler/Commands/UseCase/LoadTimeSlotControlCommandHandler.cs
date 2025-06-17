using Application.Contract.CQRS.Commands.UseCase.Commands;
using Application.Contract.Notifications.UseCase;
using Application.Services.Entities.StatisticsData;
using Application.Services.Entities.TimeSlots;
using MediatR;

namespace Application.Handler.Commands.UseCase;

public class LoadTimeSlotControlCommandHandler(
    IMediator mediator,
    IStatisticsDataRequestsService statisticsDataRequestsService,
    ITimeSlotRequestsService timeSlotRequestsService) : IRequestHandler<LoadTimeSlotControlCommand, Unit>
{
    public async Task<Unit> Handle(LoadTimeSlotControlCommand command, CancellationToken cancellationToken)
    {
        var timeSlot = await timeSlotRequestsService.GetById(command.TimeSlotId);

        var statisticsData = await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(command.TimeSlotId);

        if (statisticsData is null) throw new Exception("StatisticsData is null");

        await mediator.Publish(
            new TimeSlotControlCreatedNotification(command.ViewId, command.TimeSlotId, timeSlot.SelectedTicketId),
            cancellationToken);

        return Unit.Value;
    }
}