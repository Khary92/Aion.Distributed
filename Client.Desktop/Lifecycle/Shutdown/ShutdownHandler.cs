using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Streams;

namespace Client.Desktop.Lifecycle.Shutdown;

public class ShutdownHandler(IStreamLifeCycleHandler streamLifeCycleHandler) : IShutDownHandler
{
    public async Task Exit()
    {
        streamLifeCycleHandler.Stop();
    }
}