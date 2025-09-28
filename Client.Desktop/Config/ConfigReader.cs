using Microsoft.Extensions.Configuration;

namespace Client.Desktop.Config;

public class ConfigReader(IConfiguration config) : IConfigReader
{
    public string GetServerUrl()
    {
        var ip = config.GetValue<string>("Services:Core:Server:Address:Ip");
        var port = config.GetValue<int>("Services:Core:Server:Address:Port");
        return $"http://{ip}:{port}";
    }
}