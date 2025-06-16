using Grpc.Core;
using Proto.DTO.Tag;
using Proto.Requests.Tags;

namespace Service.Server.Mock.Tag;

public class MockTagRequestService : TagRequestService.TagRequestServiceBase
{
    public override Task<TagListProto> GetAllTags(GetAllTagsRequestProto request, ServerCallContext context)
    {
        var response = new TagListProto();
        response.Tags.Add(new TagProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Urgent",
            IsSelected = false
        });
        response.Tags.Add(new TagProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "Review",
            IsSelected = true
        });

        return Task.FromResult(response);
    }

    public override Task<TagProto> GetTagById(GetTagByIdRequestProto request, ServerCallContext context)
    {
        var tag = new TagProto
        {
            TagId = Guid.NewGuid().ToString(),
            Name = "tag-1",
            IsSelected = true
        };

        return Task.FromResult(tag);
    }
}