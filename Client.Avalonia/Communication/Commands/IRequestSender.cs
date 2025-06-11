using System.Threading.Tasks;

namespace Client.Avalonia.Communication.Commands;

public interface IRequestSender<in T>
{
    Task Send(T command);
}