using System.Threading.Tasks;
using Proto.Command.Notes;

namespace Client.Avalonia.Communication.Commands;

public interface INoteCommandSender
{
    Task<bool> Send(CreateNoteCommand command);
    Task<bool> Send(UpdateNoteCommand updateTicketDataCommand);
}