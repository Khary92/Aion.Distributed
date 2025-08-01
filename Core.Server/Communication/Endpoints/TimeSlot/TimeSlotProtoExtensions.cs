using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.TimeSlots;
using Proto.DTO.TimeSlots;
using Proto.Notifications.TimeSlots;
using Proto.Requests.TimeSlots;

namespace Core.Server.Communication.Endpoints.TimeSlot;

public static class TimeSlotProtoExtensions
{
    public static AddNoteCommand ToCommand(this AddNoteCommandProto proto) => new(Guid.Parse(proto.TimeSlotId),
        Guid.Parse(proto.NoteId), Guid.Parse(proto.TraceData.TraceId));

    public static TimeSlotNotification ToNotification(this AddNoteCommand proto) => new()
    {
        NoteAddedToTimeSlot = new NoteAddedToTimeSlotNotification
        {
            TimeSlotId = proto.TimeSlotId.ToString(),
            NoteId = proto.NoteId.ToString(),
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static SetEndTimeCommand ToCommand(this SetEndTimeCommandProto proto) => new(Guid.Parse(proto.TimeSlotId),
        proto.Time.ToDateTimeOffset(), Guid.Parse(proto.TraceData.TraceId));

    public static TimeSlotNotification ToNotification(this SetEndTimeCommand proto) => new()
    {
        EndTimeSet = new EndTimeSetNotification
        {
            TimeSlotId = proto.TimeSlotId.ToString(),
            Time = Timestamp.FromDateTimeOffset(proto.Time),
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static SetStartTimeCommand ToCommand(this SetStartTimeCommandProto proto) => new SetStartTimeCommand(
        Guid.Parse(proto.TimeSlotId), proto.Time.ToDateTimeOffset(), Guid.Parse(proto.TraceData.TraceId));

    public static TimeSlotNotification ToNotification(this SetStartTimeCommand proto) => new()
    {
        StartTimeSet = new StartTimeSetNotification
        {
            TimeSlotId = proto.TimeSlotId.ToString(),
            Time = Timestamp.FromDateTimeOffset(proto.Time),
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static CreateTimeSlotCommand ToCommand(this CreateTimeSlotCommandProto proto) => new(
        Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.SelectedTicketId),
        Guid.Parse(proto.WorkDayId), proto.StartTime.ToDateTimeOffset(), proto.EndTime.ToDateTimeOffset(),
        proto.IsTimerRunning, Guid.Parse(proto.TraceData.TraceId));

    public static TimeSlotNotification ToNotification(this CreateTimeSlotCommand proto) => new()
    {
        TimeSlotCreated = new TimeSlotCreatedNotification
        {
            TimeSlotId = proto.TimeSlotId.ToString(),
            SelectedTicketId = proto.SelectedTicketId.ToString(),
            WorkDayId = proto.WorkDayId.ToString(),
            StartTime = proto.StartTime.ToTimestamp(),
            EndTime = proto.EndTime.ToTimestamp(),
            IsTimerRunning = proto.IsTimerRunning,
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static TimeSlotProto ToProto(this Domain.Entities.TimeSlot timeSlot) => new()
    {
        TimeSlotId = timeSlot.TimeSlotId.ToString(),
        SelectedTicketId = timeSlot.SelectedTicketId.ToString(),
        WorkDayId = timeSlot.WorkDayId.ToString(),
        StartTime = timeSlot.StartTime.ToTimestamp(),
        EndTime = timeSlot.EndTime.ToTimestamp(),
        IsTimerRunning = timeSlot.IsTimerRunning
    };

    public static TimeSlotListProto ToProtoList(this List<Domain.Entities.TimeSlot> timeSlots)
    {
        var timeSlotListProto = new TimeSlotListProto();

        foreach (var timeSlot in timeSlots) timeSlotListProto.TimeSlots.Add(timeSlot.ToProto());

        return timeSlotListProto;
    }
}