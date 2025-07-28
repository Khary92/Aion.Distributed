using System.Threading;
using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Tasks.Streams;

public interface IStreamClient
{
    Task StartListening(CancellationToken cancellationToken);
}