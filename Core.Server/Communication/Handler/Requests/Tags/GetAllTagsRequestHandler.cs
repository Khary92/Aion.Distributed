using Service.Server.CQRS.Requests.Tags;
using Service.Server.Old.Services.Entities.Tags;

namespace Service.Server.Old.Handler.Requests.Tags;

public class GetAllTagsRequestHandler(ITagRequestsService tagRequestsService)
    : IRequestHandler<GetAllTagsRequest, List<TagDto>>
{
    public async Task<List<TagDto>> Handle(GetAllTagsRequest request, CancellationToken cancellationToken)
    {
        return await tagRequestsService.GetAll();
    }
}