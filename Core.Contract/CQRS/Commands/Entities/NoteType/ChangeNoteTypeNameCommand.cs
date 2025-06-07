using MediatR;

namespace Contract.CQRS.Commands.Entities.NoteType;

public record ChangeNoteTypeNameCommand(Guid NoteTypeId, string Name) : INotification, IRequest<Unit>;