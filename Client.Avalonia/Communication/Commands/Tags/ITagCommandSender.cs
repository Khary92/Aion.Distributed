using System.Threading.Tasks;
using Proto.Command.Tags;

namespace Client.Avalonia.Communication.Commands.Tags;

public interface ITagCommandSender
{
    Task<bool> Send(CreateTagCommand command);
    Task<bool> Send(UpdateTagCommand command);
}