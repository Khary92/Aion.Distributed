using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.Tags;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.Tags;

public class TagRequestSender : ITagRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TagRequestService.TagRequestServiceClient _client = new(Channel);

    public async Task<List<TagDto>> GetAllTags()
    {
        var request = new GetAllTagsRequestProto();
        var response = await _client.GetAllTagsAsync(request);
        return response.Tags.Select(tag => new TagDto(Guid.Parse(tag.TagId), tag.Name, tag.IsSelected)).ToList();
    }

    public async Task<TagDto> GetTagById(Guid tagId)
    {
        var request = new GetTagByIdRequestProto { TagId = tagId.ToString() };
        var response = await _client.GetTagByIdAsync(request);
        return new TagDto(Guid.Parse(response.TagId), response.Name, response.IsSelected);
    }
}