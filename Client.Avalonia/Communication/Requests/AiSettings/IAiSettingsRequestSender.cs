using System.Threading.Tasks;
using Proto.Requests.AiSettings;

namespace Client.Avalonia.Communication.Requests.AiSettings;

public interface IAiSettingsRequestSender
{
    Task<AiSettingsProto?> Get(string aiSettingsId);
}