using Domain.Entities;
using Domain.Events.NoteTypes;
using Domain.Interfaces;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.NoteTypes;

public class NoteTypeRequestsService(
    IEventStore<NoteTypeEvent> noteTypeEventsStore,
    IDtoMapper<NoteTypeDto, NoteType> noteTypeMapper) : INoteTypeRequestsService
{
    public async Task<List<NoteTypeDto>> GetAll()
    {
        var allEvents = await noteTypeEventsStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => NoteType.Rehydrate(group.ToList()))
            .Select(noteTypeMapper.ToDto)
            .ToList();
    }

    public async Task<NoteTypeDto?> GetById(Guid id)
    {
        var noteTypeEvents = await noteTypeEventsStore
            .GetEventsForAggregateAsync(id);

        var domainSprint = NoteType.Rehydrate(noteTypeEvents);
        return noteTypeMapper.ToDto(domainSprint);
    }
}