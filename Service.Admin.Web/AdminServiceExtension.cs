using Polly;
using Service.Admin.Web.Communication;
using Service.Admin.Web.Communication.NoteType;
using Service.Admin.Web.Communication.NoteType.State;
using Service.Admin.Web.Communication.Policies;
using Service.Admin.Web.Communication.Reports;
using Service.Admin.Web.Communication.Reports.State;
using Service.Admin.Web.Communication.Sprints;
using Service.Admin.Web.Communication.Sprints.State;
using Service.Admin.Web.Communication.Tags;
using Service.Admin.Web.Communication.Tags.State;
using Service.Admin.Web.Communication.Tickets;
using Service.Admin.Web.Communication.Tickets.State;
using Service.Admin.Web.Communication.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.State;
using Service.Admin.Web.Services;
using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Commands.TimerSettings;
using Service.Proto.Shared.Requests.NoteTypes;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tags;
using Service.Proto.Shared.Requests.Tickets;
using Service.Proto.Shared.Requests.TimerSettings;

namespace Service.Admin.Web;

public static class AdminServiceExtension
{
    private static readonly string ServerAddress = "http://core-service:8080";

    public static void AddWebServices(this IServiceCollection services)
    {
        RegisterStateServices(services);
        AddSharedDataServices(services);
        AddControllers(services);
        AddSettingsServices(services);
        AddReceiverServices(services);
        AddPolicyServices(services);
    }

    private static void AddPolicyServices(IServiceCollection services)
    {
        services.AddSingleton(
            new RetryPolicy(Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .WrapAsync(Policy
                    .Handle<Exception>()
                    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30))))
        );

        services.AddSingleton(
            new CircuitBreakerPolicy(Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30)))
        );
        
        services.AddSingleton<ISharedCommandSender, SharedCommandSender>();
        services.AddSingleton<ISharedRequestSender, SharedRequestSender>();
    }

    private static void AddReceiverServices(IServiceCollection services)
    {
        services.AddSingleton<ReportReceiver>();
        services.AddSingleton<IReportReceiver>(sp => sp.GetRequiredService<ReportReceiver>());
        
        services.AddSingleton<TicketNotificationsReceiver>();
        services.AddHostedService<TicketNotificationHostedService>();

        services.AddSingleton<TagNotificationsReceiver>();
        services.AddHostedService<TagNotificationHostedService>();

        services.AddSingleton<SprintNotificationsReceiver>();
        services.AddHostedService<SprintNotificationHostedService>();

        services.AddSingleton<NoteTypeNotificationReceiver>();
        services.AddHostedService<NoteTypeNotificationHostedService>();

        services.AddSingleton<TimerSettingsNotificationsReceiver>();
        services.AddHostedService<TimerSettingsNotificationHostedService>();
    }

    private static void AddSettingsServices(IServiceCollection services)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaxReceiveMessageSize = 2 * 1024 * 1024;
            options.MaxSendMessageSize = 2 * 1024 * 1024;
        });
    }

    private static void RegisterStateServices(IServiceCollection services)
    {
        services.AddSingleton<IComponentInitializer, ComponentInitializer>();

        services.AddSingleton<ReportStateService>();
        services.AddSingleton<IReportStateService>(sp => sp.GetRequiredService<ReportStateService>());
        
        services.AddSingleton<TicketStateService>();
        services.AddSingleton<ITicketStateService>(sp => sp.GetRequiredService<TicketStateService>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TicketStateService>());
        
        services.AddSingleton<TagStateService>();
        services.AddSingleton<ITagStateService>(sp => sp.GetRequiredService<TagStateService>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TagStateService>());
        
        services.AddSingleton<NoteTypeStateService>();
        services.AddSingleton<INoteTypeStateService>(sp => sp.GetRequiredService<NoteTypeStateService>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<NoteTypeStateService>());

        services.AddSingleton<SprintStateService>();
        services.AddSingleton<ISprintStateService>(sp => sp.GetRequiredService<SprintStateService>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<SprintStateService>());
        
        services.AddSingleton<TimerSettingsStateService>();
        services.AddSingleton<ITimerSettingsStateService>(sp => sp.GetRequiredService<TimerSettingsStateService>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TimerSettingsStateService>());
    }

    private static void AddControllers(this IServiceCollection services)
    {
        services.AddSingleton<INoteTypeController, NoteTypeController>();
        services.AddSingleton<ISprintController, SprintController>();
        services.AddSingleton<ITagController, TagController>();
        services.AddSingleton<ITicketController, TicketController>();

        services.AddSingleton<TimerSettingsController>();
        services.AddSingleton<ITimerSettingsController>(sp => sp.GetRequiredService<TimerSettingsController>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TimerSettingsController>());
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

        services.AddSingleton<ITimerSettingsCommandSender>(sp => new TimerSettingsCommandSender(ServerAddress));
        services.AddSingleton<ITimerSettingsRequestSender>(sp => new TimerSettingsRequestSender(ServerAddress));
    }
}