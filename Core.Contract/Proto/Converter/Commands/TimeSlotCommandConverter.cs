using System;
using Contract.CQRS.Commands.Entities.TimeSlots;
using Proto.Command.TimeSlots;

namespace Contract.Converters
{
    public static class TimeSlotCommandConverter
    {
        public static AddNoteProtoCommand ToProto(this AddNoteCommand command) => new()
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            NoteId = command.NoteId.ToString()
        };

        public static CreateTimeSlotProtoCommand ToProto(this CreateTimeSlotCommand command) => new()
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            SelectedTicketId = command.SelectedTicketId.ToString(),
            WorkDayId = command.WorkDayId.ToString(),
            StartTime = command.StartTime.ToString("o"),
            EndTime = command.EndTime.ToString("o"),
            IsTimerRunning = command.IsTimerRunning
        };

        public static SetEndTimeProtoCommand ToProto(this SetEndTimeCommand command) => new()
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            Time = command.Time.ToString("o")
        };

        public static SetStartTimeProtoCommand ToProto(this SetStartTimeCommand command) => new()
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            Time = command.Time.ToString("o")
        };
    }
}