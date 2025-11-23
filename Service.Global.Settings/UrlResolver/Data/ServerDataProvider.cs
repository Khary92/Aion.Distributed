using Global.Settings.Types;
using Microsoft.Extensions.Options;

namespace Global.Settings.UrlResolver.Data;

public class ServerDataProvider(IOptions<ServerSettings> serverSettings) : IGrpcDataProvider
{
    public string HostName => serverSettings.Value.HostName;
    public int GrpcPort => serverSettings.Value.GrpcPort;
    public string DockerHostName => serverSettings.Value.DockerHostName;
    public int DockerGrpcPort => serverSettings.Value.DockerGrpcPort;
    public bool IsRunningInDocker => serverSettings.Value.IsRunningInDocker;
    public ResolvingServices ResolvingServices => ResolvingServices.Server;
}