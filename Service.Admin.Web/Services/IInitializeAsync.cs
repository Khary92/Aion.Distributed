namespace Service.Admin.Web.Services;

public interface IInitializeAsync
{
   InitializationType Type { get; }
   Task InitializeComponents();
}