namespace Service.Admin.Web.Services.Startup;

public interface IComponentInitializer
{
    Task InitializeServicesAsync();
}