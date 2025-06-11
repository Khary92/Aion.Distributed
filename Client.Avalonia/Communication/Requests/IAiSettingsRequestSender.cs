using System.Threading.Tasks;
using Proto.Requests.AiSettings;

namespace Client.Avalonia.Communication.Requests;

public interface IAiSettingsRequestSender
{
    Task<AiSettingsProto?> Get(string aiSettingsId);
}