using Core.Server.Communication.Records.Commands.Entities.TimeSlots;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.TimeSlots;
using Proto.DTO.TimeSlots;
using Proto.Notifications.TimeSlots;
using Proto.Requests.TimeSlots;

namespace Core.Server.Communication.Endpoints.TimeSlot;

public static class TimeSlotProtoExtensions
{
    public static AddNoteCommand ToCommand(
        this AddNoteCommandProto proto)
    {
        return new AddNoteCommand(Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.NoteId));
    }

    public static TimeSlotNotification ToNotification(this AddNoteCommand proto)
    {
        return new TimeSlotNotification
        {
            NoteAddedToTimeSlot = new NoteAddedToTimeSlotNotification
            {
                TimeSlotId = proto.TimeSlotId.ToString(),
                NoteId = proto.NoteId.ToString()
            }
        };
    }

    public static SetEndTimeCommand ToCommand(
        this SetEndTimeCommandProto proto)
    {
        return new SetEndTimeCommand(Guid.Parse(proto.TimeSlotId), proto.Time.ToDateTimeOffset());
    }

    public static TimeSlotNotification ToNotification(this SetEndTimeCommand proto)
    {
        return new TimeSlotNotification
        {
            EndTimeSet = new EndTimeSetNotification
            {
                TimeSlotId = proto.TimeSlotId.ToString(),
                Time = Timestamp.FromDateTimeOffset(proto.Time)
            }
        };
    }

    public static SetStartTimeCommand ToCommand(
        this SetStartTimeCommandProto proto)
    {
        return new SetStartTimeCommand(Guid.Parse(proto.TimeSlotId), proto.Time.ToDateTimeOffset());
    }

    public static TimeSlotNotification ToNotification(this SetStartTimeCommand proto)
    {
        return new TimeSlotNotification
        {
            StartTimeSet = new StartTimeSetNotification
            {
                TimeSlotId = proto.TimeSlotId.ToString(),
                Time = Timestamp.FromDateTimeOffset(proto.Time)
            }
        };
    }

    public static CreateTimeSlotCommand ToCommand(
        this CreateTimeSlotCommandProto proto)
    {
        return new CreateTimeSlotCommand(Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.SelectedTicketId),
            Guid.Parse(proto.WorkDayId),
            proto.StartTime.ToDateTimeOffset(), proto.EndTime.ToDateTimeOffset(), proto.IsTimerRunning);
    }

    public static TimeSlotNotification ToNotification(this CreateTimeSlotCommand proto)
    {
        return new TimeSlotNotification
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
    }

    public static TimeSlotProto ToProto(this Domain.Entities.TimeSlot timeSlot)
    {
        return new TimeSlotProto
        {
            TimeSlotId = timeSlot.TimeSlotId.ToString(),
            SelectedTicketId = timeSlot.SelectedTicketId.ToString(),
            WorkDayId = timeSlot.WorkDayId.ToString(),
            StartTime = timeSlot.StartTime.ToTimestamp(),
            EndTime = timeSlot.EndTime.ToTimestamp(),
            IsTimerRunning = timeSlot.IsTimerRunning
        };
    }

    public static TimeSlotListProto ToProtoList(this List<Domain.Entities.TimeSlot> timeSlots)
    {
        var timeSlotListProto = new TimeSlotListProto();

        foreach (var timeSlot in timeSlots) timeSlotListProto.TimeSlots.Add(timeSlot.ToProto());

        return timeSlotListProto;
    }
}