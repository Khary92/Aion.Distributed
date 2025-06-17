namespace Application.Cache;

public interface IPersistentCache<in TCommandType>
{
    Task Persist();
    void Store(TCommandType command);
}