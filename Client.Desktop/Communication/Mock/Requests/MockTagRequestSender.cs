using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.DTO.Tag;
using Proto.Requests.Tags;
using Service.Proto.Shared.Requests.Tags;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockTagRequestSender(MockDataService mockDataService) : ITagRequestSender
{
    public Task<TagListProto> Send(GetAllTagsRequestProto request)
    {
        var result = new TagListProto
        {
            Tags = { mockDataService.Tags.Select(ConvertToProto) }
        };

        return Task.FromResult(result);
    }

    public Task<TagProto> Send(GetTagByIdRequestProto request)
    {
        return Task.FromResult(ConvertToProto(mockDataService.Tags.First(t => t.TagId.ToString() == request.TagId)));
    }

    public Task<TagListProto> Send(GetTagsByIdsRequestProto request)
    {
        var result = new TagListProto
        {
            Tags =
            {
                mockDataService.Tags.Where(t => request.TagIds.Contains(t.TagId.ToString())).Select(ConvertToProto)
            }
        };

        return Task.FromResult(result);
    }

    private static TagProto ConvertToProto(TagClientModel tagClientModel)
    {
        return new TagProto
        {
            TagId = tagClientModel.TagId.ToString(),
            Name = tagClientModel.Name,
            IsSelected = tagClientModel.IsSelected
        };
    }
}