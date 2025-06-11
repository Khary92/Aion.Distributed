using System.Threading.Tasks;
using Proto.Command.Notes;

namespace Client.Desktop.Communication.Commands.Notes;

public interface INoteCommandSender
{
    Task<bool> Send(CreateNoteCommand command);
    Task<bool> Send(UpdateNoteCommand updateTicketDataCommand);
}