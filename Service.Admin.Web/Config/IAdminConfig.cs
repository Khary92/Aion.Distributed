namespace Service.Admin.Web.Config;

public interface IAdminConfig
{
    string GetCoreServerUrl();
    public string GetMonitoringServerUrl();
}