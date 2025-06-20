using Domain.Entities;
using Domain.Events.NoteTypes;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.NoteTypes;

public class NoteTypeRequestsService(
    IEventStore<NoteTypeEvent> noteTypeEventsStore) : INoteTypeRequestsService
{
    public async Task<List<NoteType>> GetAll()
    {
        var allEvents = await noteTypeEventsStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => NoteType.Rehydrate(group.ToList())).ToList();
    }

    public async Task<NoteType?> GetById(Guid id)
    {
        var noteTypeEvents = await noteTypeEventsStore
            .GetEventsForAggregateAsync(id);

        return NoteType.Rehydrate(noteTypeEvents);
    }
}