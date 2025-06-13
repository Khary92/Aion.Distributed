using Grpc.Core;
using Proto.Requests.Tags;

public class TagRequestServiceImpl : TagRequestService.TagRequestServiceBase
{
    public override Task<TagListProto> GetAllTags(GetAllTagsRequestProto request, ServerCallContext context)
    {
        var response = new TagListProto();
        response.Tags.Add(new TagProto
        {
            TagId = "tag-1",
            Name = "Urgent",
            IsSelected = false
        });
        response.Tags.Add(new TagProto
        {
            TagId = "tag-2",
            Name = "Review",
            IsSelected = true
        });

        return Task.FromResult(response);
    }

    public override Task<TagProto> GetTagById(GetTagByIdRequestProto request, ServerCallContext context)
    {
        var tag = new TagProto
        {
            TagId = request.TagId,
            Name = request.TagId == "tag-1" ? "Urgent" : "Unknown",
            IsSelected = request.TagId == "tag-1"
        };

        return Task.FromResult(tag);
    }
}