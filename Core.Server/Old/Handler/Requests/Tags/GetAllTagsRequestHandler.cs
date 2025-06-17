using Application.Contract.CQRS.Requests.Tags;
using Application.Contract.DTO;
using Application.Services.Entities.Tags;
using MediatR;

namespace Application.Handler.Requests.Tags;

public class GetAllTagsRequestHandler(ITagRequestsService tagRequestsService)
    : IRequestHandler<GetAllTagsRequest, List<TagDto>>
{
    public async Task<List<TagDto>> Handle(GetAllTagsRequest request, CancellationToken cancellationToken)
    {
        return await tagRequestsService.GetAll();
    }
}