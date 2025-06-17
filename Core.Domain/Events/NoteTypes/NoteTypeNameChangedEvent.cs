
namespace Domain.Events.NoteTypes;

public record NoteTypeNameChangedEvent(Guid NoteTypeId, string Name);