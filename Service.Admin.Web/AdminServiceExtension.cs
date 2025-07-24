using Service.Admin.Web.Communication;
using Service.Admin.Web.Communication.NoteType.State;
using Service.Admin.Web.Communication.Reports;
using Service.Admin.Web.Communication.Reports.State;
using Service.Admin.Web.Communication.Tags;
using Service.Admin.Web.Communication.Tags.State;
using Service.Admin.Web.Communication.Tickets;
using Service.Admin.Web.Communication.Tickets.State;
using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Requests.NoteTypes;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tags;
using Service.Proto.Shared.Requests.Tickets;

namespace Service.Admin.Web;

public static class AdminServiceExtension
{
    private static readonly string ServerAddress = "http://core-service:8080";

    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddSingleton<IReportStateService, ReportStateService>();
        services.AddSingleton<ITicketStateService, TicketStateService>();
        services.AddSingleton<ITagStateService, TagStateService>();
        services.AddSingleton<INoteTypeStateService, NoteTypeStateService>();

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;
            options.MaxSendMessageSize = 2 * 1024 * 1024;
        });

        services.AddSingleton<TicketNotificationsReceiver>();
        services.AddHostedService<TicketNotificationHostedService>();

        services.AddSingleton<TagNotificationsReceiver>();
        services.AddHostedService<TagNotificationHostedService>();

        AddSharedDataServices(services);

        services.AddSingleton<ReportReceiver>();
        services.AddSingleton<IReportReceiver>(sp => sp.GetRequiredService<ReportReceiver>());

        services.AddSingleton<ISharedCommandSender, SharedCommandSender>();
        services.AddSingleton<ISharedRequestSender, SharedRequestSender>();
    }

    private static void AddSharedDataServices(this IServiceCollection services)
    {
        services.AddSingleton<ITicketCommandSender>(sp => new TicketCommandSender(ServerAddress));
        services.AddSingleton<ITicketRequestSender>(sp => new TicketRequestSender(ServerAddress));

        services.AddSingleton<ISprintCommandSender>(sp => new SprintCommandSender(ServerAddress));
        services.AddSingleton<ISprintRequestSender>(sp => new SprintRequestSender(ServerAddress));

        services.AddSingleton<ITagCommandSender>(sp => new TagCommandSender(ServerAddress));
        services.AddSingleton<ITagRequestSender>(sp => new TagRequestSender(ServerAddress));

        services.AddSingleton<INoteTypeCommandSender>(sp => new NoteTypeCommandSender(ServerAddress));
        services.AddSingleton<INoteTypeRequestSender>(sp => new NoteTypeRequestSender(ServerAddress));
    }
}