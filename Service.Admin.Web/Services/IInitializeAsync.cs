namespace Service.Admin.Web.Services;

public interface IInitializeAsync
{
    public InitializationType Type { get; }
    public Task InitializeComponents();
}