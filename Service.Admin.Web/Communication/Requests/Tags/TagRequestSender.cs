using Grpc.Net.Client;
using Proto.DTO.Tag;
using Proto.Requests.Tags;

namespace Service.Admin.Web.Communication.Requests.Tags;

public class TagRequestSender : ITagRequestSender
{
    private readonly TagProtoRequestService.TagProtoRequestServiceClient _client;

    public TagRequestSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new TagProtoRequestService.TagProtoRequestServiceClient(channel);
    }

    public async Task<TagListProto> Send(GetAllTagsRequestProto request)
    {
        return await _client.GetAllTagsAsync(request);
    }

    public async Task<TagProto> Send(GetTagByIdRequestProto request)
    {
        return await _client.GetTagByIdAsync(request);
    }

    public async Task<TagListProto> Send(GetTagsByIdsRequestProto request)
    {
        return await _client.GetTagsByIdsAsync(request);
    }
}