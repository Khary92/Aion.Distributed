using Service.Server.CQRS.Commands.UseCase.Commands;
using Service.Server.Old.Services.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.TimeSlots;

namespace Service.Server.Old.Handler.Commands.UseCase;

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