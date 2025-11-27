namespace Global.Settings;

public interface IGrpcUrlService
{
    string ClientToMonitoringUrl { get; }
    string ClientToServerUrl { get; }
    string InternalToMonitoringUrl { get; }
    string InternalToAdminUrl { get; }
    string InternalToServerUrl { get; }
}