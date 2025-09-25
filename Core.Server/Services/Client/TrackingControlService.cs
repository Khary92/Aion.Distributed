using Core.Server.Communication.Endpoints.Client;
using Core.Server.Communication.Records.Commands.Entities.StatisticsData;
using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Services.Entities.WorkDays;
using Core.Server.Tracing.Tracing.Tracers;
using Domain.Entities;
using ClientNotificationService = Core.Server.Communication.Endpoints.Client.UseCaseNotificationService;

namespace Core.Server.Services.Client;

public class TrackingControlService(
    IWorkDayRequestsService workDayRequestsService,
    ITimeSlotCommandsService timeSlotCommandsService,
    IStatisticsDataCommandsService statisticsDataCommandsService,
    ITicketRequestsService ticketRequestsService,
    ITraceCollector tracer,
    ClientNotificationService clientNotificationService) : ITrackingControlService
{
    public async Task Create(Guid ticketId, DateTimeOffset date, Guid traceId)
    {
        var workDay = await workDayRequestsService.GetWorkDayByDate(date);

        if (workDay is null) throw new Exception("WorkDay is null. Something went horribly wrong.");

        var existingTicket = await ticketRequestsService.GetTicketById(ticketId);
        if (existingTicket is null) throw new Exception("Ticket is null. Something went horribly wrong.");

        var newTimeSlot = new TimeSlot
        {
            TimeSlotId = Guid.NewGuid(),
            SelectedTicketId = ticketId,
            WorkDayId = workDay.WorkDayId,
            StartTime = DateTimeOffset.Now,
            EndTime = DateTimeOffset.Now,
            IsTimerRunning = false
        };

        await timeSlotCommandsService.Create(new CreateTimeSlotCommand(newTimeSlot.TimeSlotId,
            newTimeSlot.SelectedTicketId, newTimeSlot.WorkDayId, newTimeSlot.StartTime, newTimeSlot.EndTime,
            newTimeSlot.IsTimerRunning, traceId));

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
            newStatisticsData.TagIds, newTimeSlot.TimeSlotId, traceId));

        var clientNotification =
            ClientProtoExtensions.ToNotification(newTimeSlot, newStatisticsData, existingTicket, traceId);
        await tracer.Client.CreateTrackingControl.SendingNotification(GetType(), traceId, clientNotification);
        await clientNotificationService.SendNotificationAsync(
            clientNotification);
    }
}