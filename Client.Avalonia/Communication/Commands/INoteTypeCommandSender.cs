using System.Threading.Tasks;
using Proto.Command.NoteTypes;

namespace Client.Avalonia.Communication.Sender;

public interface INoteTypeCommandSender
{
    Task<bool> Send(CreateNoteTypeCommand command);
    Task<bool> Send(ChangeNoteTypeNameCommand command);
    Task<bool> Send(ChangeNoteTypeColorCommand command);
}