using System.Threading.Tasks;
using Proto.Command.Tags;

namespace Client.Avalonia.Communication.Commands;

public interface ITagCommandSender
{
    Task<bool> Send(CreateTagCommand command);
    Task<bool> Send(UpdateTagCommand command);
}