using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Client.Desktop.Communication.Mock;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Services.Mock;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerSeedSettingsViewModel : ReactiveObject
{
    private readonly MockDataService _mockDataService;
    private readonly IMockSeedSetupService _mockSeedSetupService;
    private readonly IStartupTask _asyncInitializeTask;

    public ServerSeedSettingsViewModel(MockDataService mockDataService, IMockSeedSetupService mockSeedSetupService, ServerSeedSettingsModel model,
        IEnumerable<IStartupTask> startupTasks)
    {
        _mockDataService = mockDataService;
        _mockSeedSetupService = mockSeedSetupService;
        _asyncInitializeTask = startupTasks.First(st => st.StartupTask == StartupTask.AsyncInitialize);

        Model = model;

        SaveSetupCommand = ReactiveCommand.CreateFromTask(Model.Save);
        ReseedCommand = ReactiveCommand.CreateFromTask(Reseed);
        ClearCommand = ReactiveCommand.CreateFromTask(Clear);
    }

    public ServerSeedSettingsModel Model { get; }

    public ReactiveCommand<Unit, Unit> SaveSetupCommand { get; }
    public ReactiveCommand<Unit, Unit> ReseedCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }

    private async Task Reseed()
    {
        await _asyncInitializeTask.Execute();
    }

    private async Task Clear()
    {
        _mockSeedSetupService.IsClearSetup = true;
        await _asyncInitializeTask.Execute();
    }
}