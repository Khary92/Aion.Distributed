using Proto.DTO.Tag;
using Proto.Requests.Tags;

namespace Service.Proto.Shared.Requests.Tags;

public interface ITagRequestSender
{
    Task<TagListProto> Send(GetAllTagsRequestProto request);
    Task<TagProto> Send(GetTagByIdRequestProto request);
    Task<TagListProto> Send(GetTagsByIdsRequestProto request);
}