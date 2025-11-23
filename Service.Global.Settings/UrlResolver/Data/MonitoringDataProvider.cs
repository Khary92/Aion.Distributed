using Global.Settings.Types;
using Microsoft.Extensions.Options;

namespace Global.Settings.UrlResolver.Data;

public class MonitoringDataProvider(IOptions<MonitoringSettings> monitoringSettings) : IGrpcDataProvider
{
    public string HostName => monitoringSettings.Value.HostName;
    public int GrpcPort => monitoringSettings.Value.GrpcPort;
    public string DockerHostName => monitoringSettings.Value.DockerHostName;
    public int DockerGrpcPort => monitoringSettings.Value.DockerGrpcPort;
    public bool IsRunningInDocker => monitoringSettings.Value.IsRunningInDocker;
    public ResolvingServices ResolvingServices => ResolvingServices.Monitoring;
}