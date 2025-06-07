using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.NoteTypes;

public record GetNoteTypeByIdRequest(Guid NoteTypeId) : IRequest<NoteTypeDto?>;