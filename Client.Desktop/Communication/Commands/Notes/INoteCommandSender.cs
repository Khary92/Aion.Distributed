using System.Threading.Tasks;
using Proto.Command.Notes;

namespace Client.Desktop.Communication.Commands.Notes;

public interface INoteCommandSender
{
    Task<bool> Send(CreateNoteCommandProto command);
    Task<bool> Send(UpdateNoteCommandProto updateTicketDataCommand);
}