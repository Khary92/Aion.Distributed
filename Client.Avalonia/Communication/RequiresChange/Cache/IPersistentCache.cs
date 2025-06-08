using System.Threading.Tasks;

namespace Client.Avalonia.Communication.RequiresChange.Cache;

public interface IPersistentCache<in TCommandType>
{
    Task Persist();
    void Store(TCommandType command);
}