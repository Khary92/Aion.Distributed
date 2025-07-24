
using Proto.Command.NoteTypes;

namespace Service.Proto.Shared.Commands.NoteTypes;

public interface INoteTypeCommandSender
{
    Task<bool> Send(CreateNoteTypeCommandProto command);
    Task<bool> Send(ChangeNoteTypeNameCommandProto command);
    Task<bool> Send(ChangeNoteTypeColorCommandProto command);
}