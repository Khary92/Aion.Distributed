using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Streams;

public interface IStreamLifeCycleHandler
{
    Task Start();
    void Stop();
}