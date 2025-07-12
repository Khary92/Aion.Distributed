using Microsoft.AspNetCore.ResponseCompression;
using Service.Admin.Web.Communication;
using Service.Admin.Web.Communication.Reports;
using Service.Admin.Web.Communication.Ticket;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tickets;

namespace Service.Admin.Web;

public static class AdminServiceExtension
{
    private static readonly string ServerAddress = "http://core-service:8080";
    
    public static void AddWebServices(this IServiceCollection services)
    {
        AddSignalRServices(services);
        
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;
            options.MaxSendMessageSize = 2 * 1024 * 1024;
        });

        services.AddSingleton<TicketNotifications>();
        services.AddHostedService<TicketNotificationHostedService>();

        AddSharedDataServices(services);
        
        services.AddSingleton<ReportReceiver>();
        services.AddSingleton<IReportReceiver>(sp => sp.GetRequiredService<ReportReceiver>());
        
        services.AddScoped<ISharedCommandSender, SharedCommandSender>();
        services.AddScoped<ISharedRequestSender, SharedRequestSender>();
    }

    private static void AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        });

        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                [ "application/octet-stream" ]);
        });
        
        services.AddSingleton<ReportHub>();
    }
    
    private static void AddSharedDataServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketCommandSender>(sp => new TicketCommandSender(ServerAddress));
        services.AddScoped<ITicketRequestSender>(sp => new TicketRequestSender(ServerAddress));

        services.AddScoped<ISprintCommandSender>(sp => new SprintCommandSender(ServerAddress));
        services.AddScoped<ISprintRequestSender>(sp => new SprintRequestSender(ServerAddress));
    }
}