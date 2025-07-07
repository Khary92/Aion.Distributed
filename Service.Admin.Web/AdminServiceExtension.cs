using Service.Admin.Web.Communication;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tickets;

namespace Service.Admin.Web;

public static class AdminServiceExtension
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;
            options.MaxSendMessageSize = 2 * 1024 * 1024;
        });

        AddSharedDataServices(services);

        services.AddSingleton<ReportEventHandler>();
        services.AddSingleton<IReportEventHandler>(sp => sp.GetRequiredService<ReportEventHandler>());
        services.AddSingleton<ReportReceiver>();
        services.AddSingleton<IReportReceiver>(sp => sp.GetRequiredService<ReportReceiver>());
        services.AddSingleton<ReportEventBridge>();

        services.AddScoped<ISharedCommandSender, SharedCommandSender>();
        services.AddScoped<ISharedRequestSender, SharedRequestSender>();
    }

    private static readonly string ServerAddress = "http://core-service:8080";
    private static void AddSharedDataServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketCommandSender>(sp => new TicketCommandSender(ServerAddress));
        services.AddScoped<ITicketRequestSender>(sp => new TicketRequestSender(ServerAddress));

        services.AddScoped<ISprintCommandSender>(sp => new SprintCommandSender(ServerAddress));
        services.AddScoped<ISprintRequestSender>(sp => new SprintRequestSender(ServerAddress));
    }
}