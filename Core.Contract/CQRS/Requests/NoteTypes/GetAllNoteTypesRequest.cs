using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.NoteTypes;

public record GetAllNoteTypesRequest : IRequest<List<NoteTypeDto>>, INotification;