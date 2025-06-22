using Core.Server.Services.Entities.Tags;
using Grpc.Core;
using Proto.DTO.Tag;
using Proto.Requests.Tags;
using Proto.Shared;

namespace Core.Server.Communication.Services.Tag;

public class TagRequestReceiver(ITagRequestsService tagRequestsService) : TagRequestService.TagRequestServiceBase
{
    public override async Task<TagListProto> GetAllTags(GetAllTagsRequestProto request, ServerCallContext context)
    {
        var tags = await tagRequestsService.GetAll();
        return tags.ToProtoList();
    }

    public override async Task<TagListProto> GetTagsByIds(GetTagsByIdsRequestProto request, ServerCallContext context)
    {
        var tags = await tagRequestsService.GetTagsByTagIds(request.TagIds.ToGuidList());
        return tags.ToProtoList();
    }

    public override async Task<TagProto> GetTagById(GetTagByIdRequestProto request, ServerCallContext context)
    {
        var tag = await tagRequestsService.GetTagById(Guid.Parse(request.TagId));
        return tag.ToProto();
    }
}