using System.Threading.Tasks;
using Proto.Command.Tags;

namespace Client.Desktop.Communication.Commands.Tags;

public interface ITagCommandSender
{
    Task<bool> Send(CreateTagCommand command);
    Task<bool> Send(UpdateTagCommand command);
}