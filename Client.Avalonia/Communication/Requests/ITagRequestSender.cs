using System.Threading.Tasks;
using Proto.Requests.Tags;

namespace Client.Avalonia.Communication.Requests;

public interface ITagRequestSender
{
    Task<TagListProto> GetAllTags();
    Task<TagProto> GetTagById(string tagId);
}