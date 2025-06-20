using Grpc.Core;
using Proto.DTO.TimeSlots;
using Proto.Requests.TimeSlots;
using Service.Server.Services.Entities.TimeSlots;

namespace Service.Server.Communication.Services.TimeSlot;

public class TimeSlotRequestReceiver(ITimeSlotRequestsService timeSlotRequestsService)
    : TimeSlotRequestService.TimeSlotRequestServiceBase
{
    public override async Task<TimeSlotProto> GetTimeSlotById(GetTimeSlotByIdRequestProto request,
        ServerCallContext context)
    {
        var timeSlot = await timeSlotRequestsService.GetById(Guid.Parse(request.TimeSlotId));
        return timeSlot.ToProto();
    }

    public override async Task<TimeSlotListProto> GetTimeSlotsForWorkDayId(GetTimeSlotsForWorkDayIdRequestProto request,
        ServerCallContext context)
    {
        var timeSlots = await timeSlotRequestsService.GetTimeSlotsForWorkDayId(Guid.Parse(request.WorkDayId));
        return timeSlots.ToProtoList();
    }
}