using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Records.Commands.UseCase.Commands;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Services.Entities.WorkDays;
using Proto.DTO.TraceData;
using Proto.Requests.UseCase;

namespace Core.Server.Communication.Endpoints.UseCase.Handler;

public class LoadTrackingControlDataHandler(
    IWorkDayRequestsService workDayRequestsService,
    ITimeSlotRequestsService timeSlotRequestsService,
    IStatisticsDataRequestsService statisticsDataRequestsService,
    ITicketRequestsService ticketRequestsService) : ILoadTrackingControlDataHandler
{
    public async Task<TrackingControlDataListProto> Handle(GetTimeSlotControlDataForDateRequest request)
    {
        var workDayByDate = await workDayRequestsService.GetWorkDayByDate(request.Date);

        if (workDayByDate == null) return new TrackingControlDataListProto();

        var timeSlots = await timeSlotRequestsService.GetTimeSlotsForWorkDayId(workDayByDate.WorkDayId);

        var timeSlotControlDataList = new TrackingControlDataListProto();
        foreach (var timeSlot in timeSlots)
        {
            var statisticsData = await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlot.TimeSlotId);
            var ticket = await ticketRequestsService.GetTicketById(timeSlot.SelectedTicketId);

            timeSlotControlDataList.TimeSlotControlData.Add(new TrackingControlDataProto
            {
                StatisticsDataProto = statisticsData.ToProto(),
                TicketProto = ticket!.ToProto(),
                TimeSlotProto = timeSlot.ToProto(),
                TraceData = new TraceDataProto
                {
                    TraceId = request.TraceId.ToString()
                }
            });
        }

        return timeSlotControlDataList;
    }
}