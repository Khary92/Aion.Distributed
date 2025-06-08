using System;
using Contract.CQRS.Commands.Entities.NoteType;
using Proto.Command.NoteType;

namespace Contract.Converters
{
    public static class NoteTypeCommandConverter
    {
        public static ChangeNoteTypeColorProtoCommand ToProto(this ChangeNoteTypeColorCommand command)
            => new()
            {
                NoteTypeId = command.NoteTypeId.ToString(),
                Color = command.Color ?? string.Empty
            };

        public static ChangeNoteTypeNameProtoCommand ToProto(this ChangeNoteTypeNameCommand command)
            => new()
            {
                NoteTypeId = command.NoteTypeId.ToString(),
                Name = command.Name ?? string.Empty
            };

        public static CreateNoteTypeProtoCommand ToProto(this CreateNoteTypeCommand command)
            => new()
            {
                NoteTypeId = command.NoteTypeId.ToString(),
                Name = command.Name ?? string.Empty,
                Color = command.Color ?? string.Empty
            };

        public static ChangeNoteTypeColorCommand ToDomain(this ChangeNoteTypeColorProtoCommand proto)
            => new(Guid.Parse(proto.NoteTypeId), proto.Color);

        public static ChangeNoteTypeNameCommand ToDomain(this ChangeNoteTypeNameProtoCommand proto)
            => new(Guid.Parse(proto.NoteTypeId), proto.Name);

        public static CreateNoteTypeCommand ToDomain(this CreateNoteTypeProtoCommand proto)
            => new(Guid.Parse(proto.NoteTypeId), proto.Name, proto.Color);
    }
}