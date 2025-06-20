using Service.Server.CQRS.Requests.Tags;
using Service.Server.Old.Services.Entities.Tags;

namespace Service.Server.Old.Handler.Requests.Tags;

public class GetTagByIdRequestHandler(ITagRequestsService tagRequestsService)
    : IRequestHandler<GetTagByIdRequest, TagDto>
{
    public async Task<TagDto> Handle(GetTagByIdRequest request, CancellationToken cancellationToken)
    {
        return await tagRequestsService.GetTagById(request.TagId);
    }
}