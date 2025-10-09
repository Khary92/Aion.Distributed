using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerSeedSettingsViewModel : ReactiveObject
{
    private readonly IStartupTask _asyncInitializeTask;

    public ServerSeedSettingsViewModel(ServerSeedSettingsModel model, IEnumerable<IStartupTask> startupTasks)
    {
        _asyncInitializeTask = startupTasks.First(st => st.StartupTask == StartupTask.AsyncInitialize);

        Model = model;

        SaveSetupCommand = ReactiveCommand.CreateFromTask(Model.Save);
        ReseedCommand = ReactiveCommand.CreateFromTask(Reseed);
    }

    public ServerSeedSettingsModel Model { get; }

    public ReactiveCommand<Unit, Unit> SaveSetupCommand { get; }
    public ReactiveCommand<Unit, Unit> ReseedCommand { get; }

    private async Task Reseed()
    {
        await _asyncInitializeTask.Execute();
    }
}