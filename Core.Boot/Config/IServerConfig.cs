namespace Core.Boot.Config;

public interface IServerConfig
{
    bool GetUseHttps();
    int GetOwnPort();
    string GetOwnAddress();

}