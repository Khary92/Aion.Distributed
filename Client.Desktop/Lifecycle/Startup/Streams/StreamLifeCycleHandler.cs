using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Scheduler;

namespace Client.Desktop.Lifecycle.Startup.Streams;

public class StreamLifeCycleHandler(
    IEnumerable<IStreamClient> receiverClients,
    CancellationTokenSource cancellationTokenSource) : IStreamLifeCycleHandler, IStartupTask
{
    public Task Start()
    {
        return Task.Run(() =>
        {
            return Task.WhenAll(receiverClients.Select(task =>
                task.StartListening(cancellationTokenSource.Token)
            ));
        });

    }

    public void Stop()
    {
        cancellationTokenSource.Cancel();
    }

    public StartupTask StartupTask => StartupTask.RegisterStreams;
    public async Task Execute()
    {
        await Start();
    }
}