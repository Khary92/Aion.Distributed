using Contract.CQRS.Commands.Entities.Note;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Note;

namespace Contract.Proto.Converter.Commands
{
    public static class NoteCommandConverter
    {
        public static CreateNoteProtoCommand ToProto(this CreateNoteCommand command)
        {
            return new CreateNoteProtoCommand
            {
                NoteId = command.NoteId.ToString(),
                Text = command.Text ?? string.Empty,
                NoteTypeId = command.NoteTypeId.ToString(),
                TimeSlotId = command.TimeSlotId.ToString(),
                TimeStamp = Timestamp.FromDateTimeOffset(command.TimeStamp)
            };
        }

        public static UpdateNoteProtoCommand ToProto(this UpdateNoteCommand command)
        {
            return new UpdateNoteProtoCommand
            {
                NoteId = command.NoteId.ToString(),
                Text = command.Text ?? string.Empty,
                NoteTypeId = command.NoteTypeId.ToString(),
                TimeSlotId = command.TimeSlotId.ToString()
            };
        }

        public static CreateNoteCommand ToDomain(this CreateNoteProtoCommand proto)
        {
            return new CreateNoteCommand(
                Guid.Parse(proto.NoteId),
                proto.Text,
                Guid.Parse(proto.NoteTypeId),
                Guid.Parse(proto.TimeSlotId),
                proto.TimeStamp.ToDateTimeOffset()
            );
        }

        public static UpdateNoteCommand ToDomain(this UpdateNoteProtoCommand proto)
        {
            return new UpdateNoteCommand(
                Guid.Parse(proto.NoteId),
                proto.Text,
                Guid.Parse(proto.NoteTypeId),
                Guid.Parse(proto.TimeSlotId)
            );
        }
    }
}