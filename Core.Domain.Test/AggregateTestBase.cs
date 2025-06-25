namespace Core.Domain.Test;

public abstract class AggregateTestBase<TEvent>
{
    protected List<TEvent> CreateEventList(params object[] domainEvents)
    {
        return domainEvents.Select(WrapEvent).ToList();
    }

    protected abstract TEvent WrapEvent(object domainEvent);
}