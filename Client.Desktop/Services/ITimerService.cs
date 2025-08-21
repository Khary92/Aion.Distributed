using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;

namespace Client.Desktop.Services;

public interface ITimerService
{
    void RegisterMessenger();
    InitializationType Type { get; }
    Task InitializeAsync();
}