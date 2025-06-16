using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.Tags;

namespace Client.Desktop.Communication.Requests.Tags;

public interface ITagRequestSender
{
    Task<List<TagDto>> Send(GetAllTagsRequestProto request);
    Task<TagDto> Send(GetTagByIdRequestProto request);
    Task<List<TagDto>> Send(GetTagsByIdsRequestProto request);
}