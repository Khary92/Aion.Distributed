using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Communication.Requests.TimeSlots.Records;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.DTO.TimeSlots;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public static class TimeSlotRequestExtensions
{
    public static GetTimeSlotByIdRequestProto ToProto(this ClientGetTimeSlotByIdRequest request)
    {
        return new GetTimeSlotByIdRequestProto
        {
            TimeSlotId = request.TimeSlotId.ToString()
        };
    }

    public static GetTimeSlotsForWorkDayIdRequestProto ToProto(this ClientGetTimeSlotsForWorkDayIdRequest request)
    {
        return new GetTimeSlotsForWorkDayIdRequestProto
        {
            WorkDayId = request.WorkDayId.ToString()
        };
    }

    public static List<TimeSlotClientModel> ToClientModelList(this TimeSlotListProto proto)
    {
        return proto.TimeSlots.Select(ToClientModel).ToList();
    }

    public static TimeSlotClientModel ToClientModel(this TimeSlotProto proto)
    {
        return new TimeSlotClientModel(
            Guid.Parse(proto.TimeSlotId),
            Guid.Parse(proto.WorkDayId),
            Guid.Parse(proto.SelectedTicketId),
            proto.StartTime.ToDateTimeOffset(),
            proto.EndTime.ToDateTimeOffset(),
            proto.NoteIds.ToGuidList(),
            proto.IsTimerRunning);
    }
}