using System.Threading.Tasks;

namespace Client.Desktop.Services.Cache;

public interface IPersistentCache<in TCommandType>
{
    Task Persist();
    void Store(TCommandType command);
}