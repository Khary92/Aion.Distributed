using Application.Contract.CQRS.Requests.NoteTypes;
using Application.Contract.DTO;
using Application.Services.Entities.NoteTypes;
using MediatR;

namespace Application.Handler.Requests.NoteTypes;

public class GetNoteTypeByIdRequestHandler(INoteTypeRequestsService noteTypeRequestsService)
    : IRequestHandler<GetNoteTypeByIdRequest, NoteTypeDto?>
{
    public async Task<NoteTypeDto?> Handle(GetNoteTypeByIdRequest request, CancellationToken cancellationToken)
    {
        return await noteTypeRequestsService.GetById(request.NoteTypeId);
    }
}