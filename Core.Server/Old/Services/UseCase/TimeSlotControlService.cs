using Service.Server.Communication.UseCase;
using Service.Server.CQRS.Commands.Entities.StatisticsData;
using Service.Server.CQRS.Commands.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.WorkDays;
using UseCaseNotificationService = Service.Server.Communication.UseCase.UseCaseNotificationService;

namespace Service.Server.Old.Services.UseCase;

public class TimeSlotControlService(
    IRunTimeSettings runTimeSettings,
    IWorkDayRequestsService workDayRequestsService,
    ITimeSlotCommandsService timeSlotCommandsService,
    IStatisticsDataCommandsService statisticsDataCommandsService,
    ITimeSlotRequestsService timeSlotRequestsService,
    IStatisticsDataRequestsService statisticsDataRequestsService,
    UseCaseNotificationService useCaseNotificationService) : ITimeSlotControlService
{
    public async Task Create(Guid ticketId)
    {
        var currentWorkDayId = (await workDayRequestsService.GetAll())
            .First(wd => wd.Date.Date == runTimeSettings.SelectedDate.Date).WorkDayId;

        var newTimeSlotId = Guid.NewGuid();
        await timeSlotCommandsService.Create(new CreateTimeSlotCommand(newTimeSlotId, ticketId,
            currentWorkDayId, DateTimeOffset.Now, DateTimeOffset.Now, false));

        var newStatisticsDataId = Guid.NewGuid();
        await statisticsDataCommandsService.Create(new CreateStatisticsDataCommand(newStatisticsDataId,
            true, false, false, [], newTimeSlotId));

        await useCaseNotificationService.SendNotificationAsync(
            UseCaseProtoExtensions.ToNotification(newTimeSlotId, newStatisticsDataId, ticketId));
    }

    public async Task Load(Guid timeSlotId)
    {
        var timeSlot = await timeSlotRequestsService.GetById(timeSlotId);

        var statisticsData = await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlotId);

        if (statisticsData is null) throw new Exception("StatisticsData is null");

        await useCaseNotificationService.SendNotificationAsync(
            UseCaseProtoExtensions.ToNotification(statisticsData.StatisticsId, timeSlotId, timeSlot.SelectedTicketId));
    }
}