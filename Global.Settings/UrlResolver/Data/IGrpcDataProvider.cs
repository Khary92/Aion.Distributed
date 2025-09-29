namespace Global.Settings.UrlResolver.Data;

public interface IGrpcDataProvider
{
    public string HostName { get; }
    public int GrpcPort { get; }
    public string DockerHostName { get; }
    public int DockerGrpcPort { get; }
    public bool IsRunningInDocker { get; }

    public ResolvingServices ResolvingServices { get; }
}