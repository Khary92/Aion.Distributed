using Global.Settings.Types;
using Microsoft.Extensions.Options;

namespace Global.Settings;

public class GrpcUrlService(
    IOptions<GlobalSettings> globalSettings,
    IOptions<AdminSettings> adminSettings,
    IOptions<MonitoringSettings> monitoringSettings,
    IOptions<ServerSettings> serverSettings) : IGrpcUrlService
{
    public string ClientToMonitoringUrl => $"https://{globalSettings.Value.ExternalHostName}:{monitoringSettings.Value.SecureExternalGrpcPort}";
    public string ClientToServerUrl => $"https://{globalSettings.Value.ExternalHostName}:{serverSettings.Value.SecureExternalGrpcPort}";

    public string InternalToMonitoringUrl => $"http://{monitoringSettings.Value.InternalHostName}:{monitoringSettings.Value.InternalGrpcPort}";
    public string InternalToAdminUrl => $"http://{adminSettings.Value.InternalHostName}:{adminSettings.Value.InternalGrpcPort}";
    public string InternalToServerUrl => $"http://{serverSettings.Value.InternalHostName}:{serverSettings.Value.InternalGrpcPort}";
}