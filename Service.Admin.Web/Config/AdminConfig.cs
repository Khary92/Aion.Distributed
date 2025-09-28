using Global.Settings.Types;
using Microsoft.Extensions.Options;

namespace Service.Admin.Web.Config;

public class AdminConfig(
    IOptions<GlobalSettings> globalSettings,
    IOptions<ServerSettings> serverSettings,
    IOptions<MonitoringSettings> monitoringSettings) : IAdminConfig
{
    public string GetCoreServerUrl()
    {
        var isDockerized = globalSettings.Value.IsRunningInDocker;
        var ip = serverSettings.Value.Host;
        var port = isDockerized ? serverSettings.Value.DockerPort : serverSettings.Value.Port;

        return GetUrl(ip, port);
    }

    public string GetMonitoringServerUrl()
    {
        var isDockerized = globalSettings.Value.IsRunningInDocker;
        var ip = monitoringSettings.Value.Host;
        var port = isDockerized ? monitoringSettings.Value.DockerPort : monitoringSettings.Value.Port;

        return GetUrl(ip, port);
    }

    private string GetUrl(string ip, int port)
    {
        var protocol = globalSettings.Value.UseHttps ? "https://" : "http://";

        return $"{protocol}{ip}:{port}";
    }
}