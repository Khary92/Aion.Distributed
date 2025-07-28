using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;

namespace Client.Desktop.Lifecycle.Shutdown;

public interface IShutDownHandler
{
    Task Exit(IClassicDesktopStyleApplicationLifetime application);
}