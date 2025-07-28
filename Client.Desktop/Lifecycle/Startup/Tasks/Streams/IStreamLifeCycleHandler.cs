using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Tasks.Streams;

public interface IStreamLifeCycleHandler
{
    Task Start();
    void Stop();
}