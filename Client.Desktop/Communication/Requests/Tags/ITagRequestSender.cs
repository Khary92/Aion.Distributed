using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.Tags;

public interface ITagRequestSender
{
    Task<List<TagDto>> GetAllTags();
    Task<TagDto> GetTagById(Guid tagId);
}