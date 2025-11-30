using Proto.Command.NoteTypes;

namespace Service.Admin.Web.Communication.Commands.NoteTypes;

public interface INoteTypeCommandSender
{
    Task<bool> Send(CreateNoteTypeCommandProto command);
    Task<bool> Send(ChangeNoteTypeNameCommandProto command);
    Task<bool> Send(ChangeNoteTypeColorCommandProto command);
}