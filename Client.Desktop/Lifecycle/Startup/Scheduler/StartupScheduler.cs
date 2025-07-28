using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Streams;

namespace Client.Desktop.Lifecycle.Startup.Scheduler;

public class StartupScheduler(IEnumerable<IStartupTask> startupTasks, IStreamLifeCycleHandler streamLifeCycleHandler)
    : IStartupScheduler
{
    private readonly List<StartupTask> _order =
    [
        StartupTask.RegisterMessenger,
        StartupTask.AsyncInitialize,
        StartupTask.CheckUnsentCommands
    ];

    private Dictionary<StartupTask, IStartupTask> LoadingStrategy =>
        startupTasks.ToDictionary(st => st.StartupTask);

    public async Task Execute()
    {
        foreach (var task in _order) await LoadingStrategy[task].Execute();
        
        // This needs to be started in a thread as this will be blocking
        _ = Task.Run(async () => await streamLifeCycleHandler.Start());
    }
}