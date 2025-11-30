using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.Tags;

namespace Client.Desktop.Communication.Requests.Tag;

public interface ITagRequestSender
{
    Task<List<TagClientModel>> Send(GetAllTagsRequestProto request);
    Task<TagClientModel> Send(GetTagByIdRequestProto request);
    Task<List<TagClientModel>> Send(GetTagsByIdsRequestProto request);
}