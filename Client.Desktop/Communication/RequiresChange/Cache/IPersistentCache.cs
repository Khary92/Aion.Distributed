using System.Threading.Tasks;

namespace Client.Desktop.Communication.RequiresChange.Cache;

public interface IPersistentCache<in TCommandType>
{
    Task Persist();
    void Store(TCommandType command);
}