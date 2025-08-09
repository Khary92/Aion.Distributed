using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Desktop.Services.Cache;

namespace Client.Desktop.Lifecycle.Shutdown;

public class ShutdownHandler(
    IStreamLifeCycleHandler streamLifeCycleHandler,
    IPersistentCache<ClientSetStartTimeCommand> startTimeCache,
    IPersistentCache<ClientSetEndTimeCommand> endTimeCache) : IShutDownHandler
{
    public async Task Exit(IClassicDesktopStyleApplicationLifetime application)
    {
        await startTimeCache.Persist();
        await endTimeCache.Persist();

        streamLifeCycleHandler.Stop();

        application.Shutdown();
    }
}