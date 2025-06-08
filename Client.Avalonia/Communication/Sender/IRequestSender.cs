using System.Threading.Tasks;

namespace Client.Avalonia.Communication.Sender;

public interface IRequestSender
{
    Task Send<T>(T command);
}