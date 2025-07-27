using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Scheduler;

public interface IStartupTask
{
    StartupTask StartupTask { get; }
    Task Execute();
}