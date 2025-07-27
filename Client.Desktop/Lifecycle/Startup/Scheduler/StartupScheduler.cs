using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Client.Desktop.Lifecycle.Startup.Scheduler;

public class StartupScheduler(IEnumerable<IStartupTask> startupTasks) : IStartupScheduler
{
    
    private readonly List<StartupTask> _order =
    [
        StartupTask.RegisterMessenger,
        StartupTask.AsyncInitialize,
        StartupTask.RegisterStreams,
        StartupTask.CheckUnsentCommands,
    ];

    private Dictionary<StartupTask, IStartupTask> LoadingStrategy =>
        startupTasks.ToDictionary(st => st.StartupTask);
    
    public async Task Execute()
    {
        foreach (var task in _order)
        {
            await LoadingStrategy[task].Execute();
        }
    }
}