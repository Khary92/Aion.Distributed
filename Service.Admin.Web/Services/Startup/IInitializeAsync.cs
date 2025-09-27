namespace Service.Admin.Web.Services.Startup;

public interface IInitializeAsync
{
    public InitializationType Type { get; }
    public Task InitializeComponents();
}