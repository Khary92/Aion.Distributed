using Global.Settings.Types;
using Microsoft.Extensions.Options;

namespace Core.Boot.Config;

public class ServerConfig(IOptions<GlobalSettings> globalSettings, IOptions<ServerSettings> serverSettings) : IServerConfig
{
    public bool GetUseHttps()
    {
        return globalSettings.Value.UseHttps;
    }

    public int GetOwnPort()
    {
        return serverSettings.Value.Port;
    }

    public string GetOwnAddress()
    {
        return serverSettings.Value.Host;
    }
}