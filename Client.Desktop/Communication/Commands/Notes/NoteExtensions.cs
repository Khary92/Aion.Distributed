using Client.Desktop.Communication.Commands.Notes.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Notes;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.Notes;

public static class NoteExtensions
{
    public static CreateNoteCommandProto ToProto(this ClientCreateNoteCommand command) => new()
    {
        NoteId = command.NoteId.ToString(),
        NoteTypeId = command.NoteTypeId.ToString(),
        Text = command.Text,
        TicketId = command.TicketId.ToString(),
        TimeSlotId = command.TimeSlotId.ToString(),
        TimeStamp = Timestamp.FromDateTimeOffset(command.TimeStamp),
        TraceData = new TraceDataProto
        {
            TraceId = command.TraceId.ToString()
        }
    };

    public static UpdateNoteCommandProto ToProto(this ClientUpdateNoteCommand command) => new()
    {
        NoteId = command.NoteId.ToString(),
        Text = command.Text,
        NoteTypeId = command.NoteTypeId.ToString(),
        TimeSlotId = command.TimeSlotId.ToString(),
        TraceData = new TraceDataProto
        {
            TraceId = command.TraceId.ToString()
        }
    };
}