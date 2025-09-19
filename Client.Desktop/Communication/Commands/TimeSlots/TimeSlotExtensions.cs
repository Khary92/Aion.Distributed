using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.TimeSlots;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.TimeSlots;

public static class TimeSlotExtensions
{
    public static AddNoteCommandProto ToProto(this ClientAddNoteCommand command)
    {
        return new AddNoteCommandProto
        {
            NoteId = command.NoteId.ToString(),
            TimeSlotId = command.TimeSlotId.ToString()
        };
    }

    public static CreateTimeSlotCommandProto ToProto(this ClientCreateTimeSlotCommand command)
    {
        return new CreateTimeSlotCommandProto
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            WorkDayId = command.WorkDayId.ToString(),
            SelectedTicketId = command.SelectedTicketId.ToString(),
            StartTime = Timestamp.FromDateTimeOffset(command.StartTime),
            EndTime = Timestamp.FromDateTimeOffset(command.EndTime),
            IsTimerRunning = command.IsTimerRunning
        };
    }

    public static SetStartTimeCommandProto ToProto(this ClientSetStartTimeCommand command)
    {
        return new SetStartTimeCommandProto
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            Time = Timestamp.FromDateTimeOffset(command.Time),
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static SetEndTimeCommandProto ToProto(this ClientSetEndTimeCommand command)
    {
        return new SetEndTimeCommandProto
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            Time = Timestamp.FromDateTimeOffset(command.Time),
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }
}