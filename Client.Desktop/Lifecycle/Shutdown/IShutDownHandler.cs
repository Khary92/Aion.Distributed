using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Shutdown;

public interface IShutDownHandler
{
    void Exit();
    Task ExitAndSendCommands();
}