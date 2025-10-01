using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Tasks.Streams;

public class StreamLifeCycleHandler(
    IEnumerable<IStreamClient> receiverClients,
    CancellationTokenSource cancellationTokenSource) : IStreamLifeCycleHandler
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
}