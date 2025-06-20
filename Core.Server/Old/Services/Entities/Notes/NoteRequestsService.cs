using Domain.Entities;
using Domain.Events.Note;
using Domain.Interfaces;

namespace Service.Server.Old.Services.Entities.Notes;

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

        var notesByTimeSlotId = groupedEvents
            .Select(group => Note.Rehydrate(group.ToList()));

        //TODO: This is bad. I can't filter by ticketId because notes do not have a ticketId. Maybe i need to remodel that...
        return new List<Note>();
    }
}