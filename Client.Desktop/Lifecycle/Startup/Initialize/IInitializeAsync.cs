using System.Threading.Tasks;

namespace Client.Desktop.Lifecycle.Startup.Initialize;

public interface IInitializeAsync
{
    public InitializationType Type { get; }
    Task InitializeAsync();
}