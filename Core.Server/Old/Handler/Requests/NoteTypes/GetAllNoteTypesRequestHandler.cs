using Application.Contract.CQRS.Requests.NoteTypes;
using Application.Contract.DTO;
using Application.Services.Entities.NoteTypes;
using MediatR;

namespace Application.Handler.Requests.NoteTypes;

public class GetAllNoteTypesRequestHandler(INoteTypeRequestsService noteTypeRequestsService)
    : IRequestHandler<GetAllNoteTypesRequest, List<NoteTypeDto>>
{
    public async Task<List<NoteTypeDto>> Handle(GetAllNoteTypesRequest request, CancellationToken cancellationToken)
    {
        return await noteTypeRequestsService.GetAll();
    }
}