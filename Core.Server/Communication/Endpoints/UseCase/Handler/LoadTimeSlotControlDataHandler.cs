using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Records.Commands.UseCase.Commands;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Services.Entities.WorkDays;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Endpoints.UseCase.Handler;

public class LoadTimeSlotControlDataHandler(
    IWorkDayRequestsService workDayRequestsService,
    ITimeSlotRequestsService timeSlotRequestsService,
    IStatisticsDataRequestsService statisticsDataRequestsService,
    ITicketRequestsService ticketRequestsService)
{
    public async Task<TimeSlotControlDataListProto> Handle(GetTimeSlotControlDataForDateRequest request)
    {
        var workDayByDate = await workDayRequestsService.GetWorkDayByDate(request.Date);

        if (workDayByDate == null) return new TimeSlotControlDataListProto();

        var timeSlots = await timeSlotRequestsService.GetTimeSlotsForWorkDayId(workDayByDate.WorkDayId);

        var timeSlotControlDataList = new TimeSlotControlDataListProto();
        foreach (var timeSlot in timeSlots)
        {
            var statisticsData = await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlot.TimeSlotId);
            var ticket = await ticketRequestsService.GetTicketById(timeSlot.SelectedTicketId);

            timeSlotControlDataList.TimeSlotControlData.Add(new TimeSlotControlDataProto
            {
                StatisticsDataProto = statisticsData.ToProto(),
                TicketProto = ticket!.ToProto(),
                TimeSlotProto = timeSlot.ToProto(),
                TraceData = new()
                {
                    TraceId = request.TraceId.ToString()
                }
            });
        }

        return timeSlotControlDataList;
    }
}