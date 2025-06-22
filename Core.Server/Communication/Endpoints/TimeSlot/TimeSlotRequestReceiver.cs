using Core.Server.Services.Entities.TimeSlots;
using Grpc.Core;
using Proto.DTO.TimeSlots;
using Proto.Requests.TimeSlots;

namespace Core.Server.Communication.Endpoints.TimeSlot;

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