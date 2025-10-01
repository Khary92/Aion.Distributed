using Global.Settings.Types;
using Microsoft.Extensions.Options;

namespace Global.Settings.UrlResolver.Data;

public class AdminDataProvider(IOptions<AdminSettings> adminSettings) : IGrpcDataProvider
{
    public string HostName => adminSettings.Value.HostName;
    public int GrpcPort => adminSettings.Value.GrpcPort;
    public string DockerHostName => adminSettings.Value.DockerHostName;
    public int DockerGrpcPort => adminSettings.Value.DockerGrpcPort;
    public bool IsRunningInDocker => adminSettings.Value.IsRunningInDocker;
    public ResolvingServices ResolvingServices => ResolvingServices.WebAdmin;
}