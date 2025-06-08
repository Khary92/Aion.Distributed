using System.Threading.Tasks;

namespace Client.Avalonia.Communication.Sender;

public interface IRequestSender<in T>
{
    Task Send(T command);
}