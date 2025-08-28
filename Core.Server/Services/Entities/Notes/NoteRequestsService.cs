using Domain.Entities;
using Domain.Events.Note;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Notes;

public class NoteRequestsService(IEventStore<NoteEvent> noteEventsStore)
    : INoteRequestsService
{
    public async Task<List<Note>> GetNotesByTimeSlotId(Guid timeSlotId)
    {
        var allEvents = await noteEventsStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .ToList();

        var notesByTimeSlotId = groupedEvents
            .Select(group => Note.Rehydrate(group.ToList()));

        return notesByTimeSlotId.Where(t => t.TimeSlotId == timeSlotId)
            .ToList();
    }

    public async Task<List<Note>> GetNotesByTicketId(Guid ticketId)
    {
        var allEvents = await noteEventsStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .ToList();

        return groupedEvents
            .Select(group => Note.Rehydrate(group.ToList()))
            .Where(t => t.TicketId == ticketId).ToList();
    }
}