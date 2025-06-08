using System.Threading.Tasks;

namespace Client.Avalonia.Communication.Sender;

public interface ICommandSender
{
    Task Send<T>(T command);
}