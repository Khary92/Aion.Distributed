using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Desktop.Services.Cache;
using Microsoft.Extensions.Hosting;

namespace Client.Desktop.Lifecycle.Shutdown;

public class ShutdownHandler(
    IHostApplicationLifetime applicationLifetime,
    IPersistentCache<ClientSetStartTimeCommand> startTimeCache,
    IPersistentCache<ClientSetEndTimeCommand> endTimeCache,
    IDisposable timerService,
    IEnumerable<IEventRegistration> messengers,
    IStreamLifeCycleHandler streamLifeCycleHandler) : IShutDownHandler
{
    public void Exit()
    {
        streamLifeCycleHandler.Stop();

        foreach (var messenger in messengers) messenger.UnregisterMessenger();

        timerService.Dispose();

        applicationLifetime.StopApplication();

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
            desktopApp.Shutdown();
    }

    public async Task ExitAndSendCommands()
    {
        await startTimeCache.Persist();
        await endTimeCache.Persist();
        Exit();
    }
}