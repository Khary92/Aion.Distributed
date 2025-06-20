using Google.Protobuf.WellKnownTypes;
using Proto.Command.TimeSlots;
using Proto.DTO.TimeSlots;
using Proto.Notifications.TimeSlots;
using Proto.Requests.TimeSlots;
using Service.Server.Communication.CQRS.Commands.Entities.TimeSlots;

namespace Service.Server.Communication.Services.TimeSlot;

public static class TimeSlotProtoExtensions
{
    public static AddNoteCommand ToCommand(
        this AddNoteCommandProto proto) =>
        new(Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.NoteId));

    public static TimeSlotNotification ToNotification(this AddNoteCommand proto) =>
        new()
        {
            NoteAddedToTimeSlot = new NoteAddedToTimeSlotNotification
            {
                TimeSlotId = proto.TimeSlotId.ToString(),
                NoteId = proto.NoteId.ToString(),
            }
        };

    public static SetEndTimeCommand ToCommand(
        this SetEndTimeCommandProto proto) =>
        new(Guid.Parse(proto.TimeSlotId), proto.Time.ToDateTimeOffset());

    public static TimeSlotNotification ToNotification(this SetEndTimeCommand proto) =>
        new()
        {
            EndTimeSet = new EndTimeSetNotification
            {
                TimeSlotId = proto.TimeSlotId.ToString(),
                Time = Timestamp.FromDateTimeOffset(proto.Time)
            }
        };

    public static SetStartTimeCommand ToCommand(
        this SetStartTimeCommandProto proto) =>
        new(Guid.Parse(proto.TimeSlotId), proto.Time.ToDateTimeOffset());

    public static TimeSlotNotification ToNotification(this SetStartTimeCommand proto) =>
        new()
        {
            StartTimeSet = new StartTimeSetNotification
            {
                TimeSlotId = proto.TimeSlotId.ToString(),
                Time = Timestamp.FromDateTimeOffset(proto.Time)
            }
        };

    public static CreateTimeSlotCommand ToCommand(
        this CreateTimeSlotCommandProto proto) =>
        new(Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.SelectedTicketId), Guid.Parse(proto.WorkDayId),
            proto.StartTime.ToDateTimeOffset(), proto.EndTime.ToDateTimeOffset(), proto.IsTimerRunning);

    public static TimeSlotNotification ToNotification(this CreateTimeSlotCommand proto) =>
        new()
        {
            TimeSlotCreated = new TimeSlotCreatedNotification
            {
                TimeSlotId = proto.TimeSlotId.ToString(),
                SelectedTicketId = proto.SelectedTicketId.ToString(),
                WorkDayId = proto.WorkDayId.ToString(),
                StartTime = proto.StartTime.ToTimestamp(),
                EndTime = proto.EndTime.ToTimestamp(),
                IsTimerRunning = proto.IsTimerRunning
            }
        };

    public static TimeSlotProto ToProto(this Domain.Entities.TimeSlot timeSlot) =>
        new()
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

        foreach (var timeSlot in timeSlots)
        {
            timeSlotListProto.TimeSlots.Add(timeSlot.ToProto());
        }

        return timeSlotListProto;
    }
}