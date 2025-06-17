namespace Domain.Events.NoteTypes;

public record NoteTypeCreatedEvent(Guid NoteTypeId, string Name, string Color);