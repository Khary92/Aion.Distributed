using Service.Server.CQRS.Requests.NoteTypes;
using Service.Server.Old.Services.Entities.NoteTypes;

namespace Service.Server.Old.Handler.Requests.NoteTypes;

public class GetNoteTypeByIdRequestHandler(INoteTypeRequestsService noteTypeRequestsService)
    : IRequestHandler<GetNoteTypeByIdRequest, NoteTypeDto?>
{
    public async Task<NoteTypeDto?> Handle(GetNoteTypeByIdRequest request, CancellationToken cancellationToken)
    {
        return await noteTypeRequestsService.GetById(request.NoteTypeId);
    }
}