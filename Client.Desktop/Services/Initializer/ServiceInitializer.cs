using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Desktop.Services.Initializer;

public class ServiceInitializer(IEnumerable<IInitializeAsync> components) : IServiceInitializer
{
    public async Task InitializeServicesAsync()
    {
        foreach (var component in components)
        {
            await component.InitializeAsync();
        }
    }
}