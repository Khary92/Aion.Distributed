using System.Threading.Tasks;

namespace Client.Desktop.Services.Initializer;

public interface IServiceInitializer
{
    Task InitializeServicesAsync();
}