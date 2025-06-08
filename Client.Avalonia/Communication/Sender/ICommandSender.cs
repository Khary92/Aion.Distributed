using System.Threading.Tasks;
using Proto.Command;

namespace Client.Avalonia.Communication.Sender;

public interface ICommandSender<in T>
{
    Task<CommandResponse> Send(T command);
}