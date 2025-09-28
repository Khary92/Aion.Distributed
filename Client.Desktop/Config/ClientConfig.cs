using Global.Settings.Types;
using Microsoft.Extensions.Options;

namespace Client.Desktop.Config;

public class ClientConfig(
    IOptions<GlobalSettings> globalSettings,
    IOptions<ServerSettings> serverSettings) : IClientConfig
{
    public string GetCoreServerUrl()
    {
        var isDockerized = globalSettings.Value.IsRunningInDocker;
        var ip = isDockerized ? serverSettings.Value.DockerHostnameForClient : serverSettings.Value.Host;
        var port = isDockerized ? serverSettings.Value.DockerPort : serverSettings.Value.Port;

        return GetUrl(ip, port);
    }

    private string GetUrl(string ip, int port)
    {
        var protocol = globalSettings.Value.UseHttps ? "https://" : "http://";

        return $"{protocol}{ip}:{port}";
    }
}