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
    IEnumerable<IMessengerRegistration> messengers,
    IStreamLifeCycleHandler streamLifeCycleHandler) : IShutDownHandler
{
    public async Task Exit()
    {
        await startTimeCache.Persist();
        await endTimeCache.Persist();

        streamLifeCycleHandler.Stop();

        foreach (var messenger in messengers) messenger.UnregisterMessenger();

        try
        {
            timerService.Dispose();
        }
        catch (ObjectDisposedException)
        {
        }

        applicationLifetime.StopApplication();

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopApp)
            desktopApp.Shutdown();
    }
}