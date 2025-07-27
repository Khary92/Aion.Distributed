using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes.Records;
using Proto.Command.Notes;

namespace Client.Desktop.Communication.Commands.Notes;

public interface INoteCommandSender
{
    Task<bool> Send(ClientCreateNoteCommand command);
    Task<bool> Send(ClientUpdateNoteCommand command);
}