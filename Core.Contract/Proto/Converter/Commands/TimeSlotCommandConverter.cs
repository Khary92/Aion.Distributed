using Contract.Cqrs.Commands.Entities.Timeslots;
using Contract.CQRS.Commands.Entities.TimeSlots;
using Google.Protobuf.WellKnownTypes;

namespace Contract.Proto.Converter.Commands
{
    public static class TimeSlotsCommandConverter
    {
        public static AddNoteCommandProto ToProto(this AddNoteCommand command)
            => new()
            {
                TimeSlotId = command.TimeSlotId.ToString(),
                NoteId = command.NoteId.ToString()
            };

        public static CreateTimeSlotCommandProto ToProto(this CreateTimeSlotCommand command)
            => new()
            {
                TimeSlotId = command.TimeSlotId.ToString(),
                SelectedTicketId = command.SelectedTicketId.ToString(),
                WorkDayId = command.WorkDayId.ToString(),
                StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
                EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
                IsTimerRunning = command.IsTimerRunning
            };

        public static SetEndTimeCommandProto ToProto(this SetEndTimeCommand command)
            => new()
            {
                TimeSlotId = command.TimeSlotId.ToString(),
                Time = Timestamp.FromDateTimeOffset(command.Time)
            };

        public static SetStartTimeCommandProto ToProto(this SetStartTimeCommand command)
            => new()
            {
                TimeSlotId = command.TimeSlotId.ToString(),
                Time = Timestamp.FromDateTimeOffset(command.Time)
            };

        public static AddNoteCommand ToDomain(this AddNoteCommandProto proto)
            => new(Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.NoteId));

        public static CreateTimeSlotCommand ToDomain(this CreateTimeSlotCommandProto proto)
            => new(
                Guid.Parse(proto.TimeSlotId),
                Guid.Parse(proto.SelectedTicketId),
                Guid.Parse(proto.WorkDayId),
                proto.StartTime.ToDateTimeOffset(),
                proto.EndTime.ToDateTimeOffset(),
                proto.IsTimerRunning);

        public static SetEndTimeCommand ToDomain(this SetEndTimeCommandProto proto)
            => new(Guid.Parse(proto.TimeSlotId), proto.Time.ToDateTimeOffset());

        public static SetStartTimeCommand ToDomain(this SetStartTimeCommandProto proto)
            => new(Guid.Parse(proto.TimeSlotId), proto.Time.ToDateTimeOffset());
    }
}