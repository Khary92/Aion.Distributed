using Global.Settings.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Service.Monitoring.Config;

public class MonitoringConfig(
    IOptions<GlobalSettings> globalSettings,
    IOptions<AdminSettings> adminSettings) : IMonitoringConfig
{
    public string GetWebAdminServerUrl()
    {
        var isDockerized = globalSettings.Value.IsRunningInDocker;
        var ip = adminSettings.Value.Host;
        var port = isDockerized ? adminSettings.Value.DockerPort : adminSettings.Value.Port;

        return GetUrl(ip, port);
    }

    private string GetUrl(string ip, int port)
    {
        var protocol = globalSettings.Value.UseHttps ? "https://" : "http://";

        return $"{protocol}{ip}:{port}";
    }
}