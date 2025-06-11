using System.Threading.Tasks;
using Proto.Requests.Settings;

namespace Client.Avalonia.Communication.Requests.Settings;

public interface ISettingsRequestSender
{
    Task<SettingsProto> GetSettings();
    Task<bool> IsExportPathValid();
}