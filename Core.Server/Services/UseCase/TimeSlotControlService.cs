using Core.Server.Communication.Endpoints.UseCase;
using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Services.Entities.WorkDays;
using Domain.Entities;
using UseCaseNotificationService = Core.Server.Communication.Endpoints.UseCase.UseCaseNotificationService;

namespace Core.Server.Services.UseCase;

public class TimeSlotControlService(
    IRunTimeSettings runTimeSettings,
    IWorkDayRequestsService workDayRequestsService,
    ITimeSlotCommandsService timeSlotCommandsService,
    IStatisticsDataCommandsService statisticsDataCommandsService,
    ITimeSlotRequestsService timeSlotRequestsService,
    IStatisticsDataRequestsService statisticsDataRequestsService,
    ITicketRequestsService ticketRequestsService,
    UseCaseNotificationService useCaseNotificationService) : ITimeSlotControlService
{
    public async Task Create(Guid ticketId)
    {
        // TODO RuntimeSettings are meant for client only. Not for the server side.
        var workDays = await workDayRequestsService.GetAll();
        var currentWorkDayId = workDays
            .First(wd => wd.Date.Date == runTimeSettings.SelectedDate.Date).WorkDayId;

        var existingTicket = await ticketRequestsService.GetTicketById(ticketId);
        if (existingTicket is null) throw new Exception("Ticket is null. Something went horribly wrong.");

        var newTimeSlot = new TimeSlot
        {
            TimeSlotId = Guid.NewGuid(),
            SelectedTicketId = ticketId,
            WorkDayId = currentWorkDayId,
            StartTime = DateTimeOffset.Now,
            EndTime = DateTimeOffset.Now,
            IsTimerRunning = false
        };

        await timeSlotCommandsService.Create(new CreateTimeSlotCommand(newTimeSlot.TimeSlotId,
            newTimeSlot.SelectedTicketId, newTimeSlot.WorkDayId, newTimeSlot.StartTime, newTimeSlot.EndTime,
            newTimeSlot.IsTimerRunning, Guid.NewGuid()));

        var newStatisticsData = new StatisticsData
        {
            StatisticsId = Guid.NewGuid(),
            IsProductive = true,
            IsNeutral = false,
            IsUnproductive = false,
            TagIds = []
        };

        await statisticsDataCommandsService.Create(new CreateStatisticsDataCommand(newStatisticsData.StatisticsId,
            newStatisticsData.IsProductive, newStatisticsData.IsNeutral, newStatisticsData.IsUnproductive,
            newStatisticsData.TagIds, newTimeSlot.TimeSlotId, Guid.NewGuid()));

        await useCaseNotificationService.SendNotificationAsync(
            UseCaseProtoExtensions.ToNotification(newTimeSlot, newStatisticsData, existingTicket));
    }

    public async Task Load(Guid timeSlotId)
    {
        var timeSlot = await timeSlotRequestsService.GetById(timeSlotId);
        if (timeSlot is null) throw new Exception("TimeSlot is null. Something went horribly wrong.");

        var ticket = await ticketRequestsService.GetTicketById(timeSlot.SelectedTicketId);
        if (ticket is null) throw new Exception("Ticket is null. Something went horribly wrong.");

        var statisticsData = await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlotId);
        if (statisticsData is null) throw new Exception("StatisticsData is null. Something went horribly wrong.");

        await useCaseNotificationService.SendNotificationAsync(
            UseCaseProtoExtensions.ToNotification(timeSlot, statisticsData, ticket));
    }
}