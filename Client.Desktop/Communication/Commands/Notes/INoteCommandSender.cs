using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.Notes.Records;

namespace Client.Desktop.Communication.Commands.Notes;

public interface INoteCommandSender
{
    Task<bool> Send(ClientCreateNoteCommand command);
    Task<bool> Send(ClientUpdateNoteCommand command);
}