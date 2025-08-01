using Domain.Events;

namespace Domain.Interfaces;

public interface IEventStore<TEvent> where TEvent : IDomainEvent
{
    Task StoreEventAsync(TEvent @event, Guid TraceId);
    Task<List<TEvent>> GetEventsForAggregateAsync(Guid entityId);
    Task<List<TEvent>> GetAllEventsAsync();
}