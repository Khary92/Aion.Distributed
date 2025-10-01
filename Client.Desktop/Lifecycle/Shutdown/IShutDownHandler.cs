using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Shutdown;

public interface IShutDownHandler
{
    Task Exit();
}