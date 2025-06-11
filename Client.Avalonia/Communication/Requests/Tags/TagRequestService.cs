using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.Tags;

namespace Client.Avalonia.Communication.Requests.Tags;

public class TagRequestSender : ITagRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TagRequestService.TagRequestServiceClient _client = new(Channel);

    public async Task<TagListProto> GetAllTags()
    {
        var request = new GetAllTagsRequestProto();
        var response = await _client.GetAllTagsAsync(request);
        return response;
    }

    public async Task<TagProto> GetTagById(string tagId)
    {
        var request = new GetTagByIdRequestProto { TagId = tagId };
        var response = await _client.GetTagByIdAsync(request);
        return response;
    }
}