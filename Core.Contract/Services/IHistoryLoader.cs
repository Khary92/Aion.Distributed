namespace Contract.Services;

public interface IHistoryLoader<TDto>
{
    Task<IEnumerable<TDto>> Load(Guid id);
}