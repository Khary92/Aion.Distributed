using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.AiSettings;

namespace Client.Avalonia.Communication.Requests.AiSettings;

public interface IAiSettingsRequestSender
{
    Task<AiSettingsDto?> Get(string aiSettingsId);
}