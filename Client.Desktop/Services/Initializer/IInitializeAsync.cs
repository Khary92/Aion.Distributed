using System.Threading.Tasks;

namespace Client.Desktop.Services.Initializer;

public interface IInitializeAsync
{
    public InitializationType Type { get; }
    Task InitializeAsync();
}