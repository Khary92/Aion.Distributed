using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Services.Cache;

namespace Client.Desktop.Lifecycle.Startup.Tasks.UnsentCommands;

public class SendUnsentCommandsTask(
    IPersistentCache<ClientSetStartTimeCommand> startTimeCache,
    IPersistentCache<ClientSetEndTimeCommand> endTimeCache) : IStartupTask
{
    public StartupTask StartupTask => StartupTask.CheckUnsentCommands;

    public async Task Execute()
    {
        await startTimeCache.Persist();
        await endTimeCache.Persist();
    }
}