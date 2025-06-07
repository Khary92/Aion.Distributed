using MediatR;

namespace Contract.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeColorCommand(Guid NoteTypeId, string Color) : INotification, IRequest<Unit>;