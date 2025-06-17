using Application.Contract.CQRS.Requests.Tags;
using Application.Contract.DTO;
using Application.Services.Entities.Tags;
using MediatR;

namespace Application.Handler.Requests.Tags;

public class GetTagByIdRequestHandler(ITagRequestsService tagRequestsService)
    : IRequestHandler<GetTagByIdRequest, TagDto>
{
    public async Task<TagDto> Handle(GetTagByIdRequest request, CancellationToken cancellationToken)
    {
        return await tagRequestsService.GetTagById(request.TagId);
    }
}