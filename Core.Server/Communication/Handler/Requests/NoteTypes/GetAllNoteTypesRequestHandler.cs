using Service.Server.CQRS.Requests.NoteTypes;
using Service.Server.Old.Services.Entities.NoteTypes;

namespace Service.Server.Old.Handler.Requests.NoteTypes;

public class GetAllNoteTypesRequestHandler(INoteTypeRequestsService noteTypeRequestsService)
    : IRequestHandler<GetAllNoteTypesRequest, List<NoteTypeDto>>
{
    public async Task<List<NoteTypeDto>> Handle(GetAllNoteTypesRequest request, CancellationToken cancellationToken)
    {
        return await noteTypeRequestsService.GetAll();
    }
}