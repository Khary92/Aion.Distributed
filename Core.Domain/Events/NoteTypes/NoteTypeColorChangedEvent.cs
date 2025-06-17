
namespace Domain.Events.NoteTypes;

public record NoteTypeColorChangedEvent(Guid NoteTypeId, string Color);