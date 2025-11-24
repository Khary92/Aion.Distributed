using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Scheduler;

public interface IEventRegistration
{
    Task Execute();
}