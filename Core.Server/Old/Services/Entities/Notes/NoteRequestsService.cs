using Domain.Entities;
using Domain.Events.Note;
using Domain.Interfaces;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.Notes;

public class NoteRequestsService(IEventStore<NoteEvent> noteEventsStore, IDtoMapper<NoteDto, Note> noteMapper)
    : INoteRequestsService
{
    public async Task<List<NoteDto>> GetNotesByTimeSlotId(Guid timeSlotId)
    {
        var allEvents = await noteEventsStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .ToList();

        var notesByTimeSlotId = groupedEvents
            .Select(group => Note.Rehydrate(group.ToList()))
            .Select(noteMapper.ToDto);

        return notesByTimeSlotId.Where(t => t.TimeSlotId == timeSlotId)
            .ToList();
    }
}