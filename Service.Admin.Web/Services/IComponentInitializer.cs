namespace Service.Admin.Web.Services;

public interface IComponentInitializer
{
    Task InitializeServicesAsync();
}