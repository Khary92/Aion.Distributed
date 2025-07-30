using Client.Desktop.Communication.Commands.Notes.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Notes;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.Notes;

public static class NoteExtensions
{
    public static CreateNoteCommandProto ToProto(this ClientCreateNoteCommand command)
    {
        return new CreateNoteCommandProto
        {
            NoteId = command.NoteId.ToString(),
            NoteTypeId = command.NoteTypeId.ToString(),
            Text = command.Text,
            TimeSlotId = command.TimeSlotId.ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(command.TimeStamp),
            TraceData = new TraceDataProto()
            {
                
            }
        };
    }
    
    public static UpdateNoteCommandProto ToProto(this ClientUpdateNoteCommand command)
    {
        return new UpdateNoteCommandProto
        {
            NoteId = command.NoteId.ToString(),
            Text = command.Text,
            NoteTypeId = command.NoteTypeId.ToString(),
            TimeSlotId = command.TimeSlotId.ToString()
        };
    }
}