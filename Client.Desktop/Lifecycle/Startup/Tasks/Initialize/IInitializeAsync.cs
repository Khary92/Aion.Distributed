using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Tasks.Initialize;

public interface IInitializeAsync
{
    public InitializationType Type { get; }
    public Task InitializeAsync();
}