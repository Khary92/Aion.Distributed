using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Scheduler;

public class EventRegistration(
    IEnumerable<IStartupTask> startupTasks) : IEventRegistration
{
    private Dictionary<StartupTask, IStartupTask> LoadingStrategy =>
        startupTasks.ToDictionary(st => st.StartupTask);

    public async Task Execute()
    {
        await LoadingStrategy[StartupTask.RegisterMessenger].Execute();
    }
}