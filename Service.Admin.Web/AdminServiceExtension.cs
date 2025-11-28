using Global.Settings;
using Grpc.Core;
using Polly;
using Service.Admin.Web.Communication.Commands.NoteTypes;
using Service.Admin.Web.Communication.Commands.Sprints;
using Service.Admin.Web.Communication.Commands.Tags;
using Service.Admin.Web.Communication.Commands.Tickets;
using Service.Admin.Web.Communication.Commands.TimerSettings;
using Service.Admin.Web.Communication.Receiver;
using Service.Admin.Web.Communication.Receiver.Reports;
using Service.Admin.Web.Communication.Requests.NoteTypes;
using Service.Admin.Web.Communication.Requests.Sprints;
using Service.Admin.Web.Communication.Requests.Tags;
using Service.Admin.Web.Communication.Requests.Tickets;
using Service.Admin.Web.Communication.Requests.TimerSettings;
using Service.Admin.Web.Communication.Sender;
using Service.Admin.Web.Communication.Sender.Common;
using Service.Admin.Web.Communication.Sender.Policies;
using Service.Admin.Web.Services.Startup;
using Service.Admin.Web.Services.State;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Web;

public static class AdminServiceExtension
{
    public static void AddWebServices(this IServiceCollection services)
    {
        AddTraceSender(services);
        RegisterStateServices(services);
        AddSharedDataServices(services);
        AddControllers(services);
        AddSettingsServices(services);
        AddReceiverServices(services);
        AddPolicyServices(services);
    }

    private static void AddTraceSender(this IServiceCollection services)
    {
        services.AddSingleton<ITracingDataSender>(sp =>
            new TracingDataSender(sp.GetRequiredService<IGrpcUrlService>().InternalToMonitoringUrl));
    }

    private static void AddPolicyServices(IServiceCollection services)
    {
        services.AddSingleton(
            new CommandSenderPolicy(Policy
                .Handle<RpcException>()
                .WaitAndRetryAsync(
                    5,
                    retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                ))
        );

        services.AddSingleton(
            new RequestSenderPolicy(Policy
                .Handle<RpcException>()
                .WaitAndRetryAsync(
                    5,
                    retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                ))
        );

        services.AddSingleton<ISharedCommandSender, SharedCommandSender>();
        services.AddSingleton<ISharedRequestSender, SharedRequestSender>();
    }

    private static void AddReceiverServices(IServiceCollection services)
    {
        services.AddSingleton<ReportReceiver>();

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

        services.AddSingleton<IReportStateServiceFactory, ReportStateServiceFactory>();

        foreach (var sortingType in Enum.GetValues<SortingType>())
            services.AddSingleton<IReportStateService>(_ => new ReportStateService(sortingType));

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
        services.AddSingleton<ITicketCommandSender>(sp =>
            new TicketCommandSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));
        services.AddSingleton<ITicketRequestSender>(sp =>
            new TicketRequestSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));

        services.AddSingleton<ISprintCommandSender>(sp =>
            new SprintCommandSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));
        services.AddSingleton<ISprintRequestSender>(sp =>
            new SprintRequestSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));

        services.AddSingleton<ITagCommandSender>(sp =>
            new TagCommandSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));
        services.AddSingleton<ITagRequestSender>(sp =>
            new TagRequestSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));

        services.AddSingleton<INoteTypeCommandSender>(sp =>
            new NoteTypeCommandSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));
        services.AddSingleton<INoteTypeRequestSender>(sp =>
            new NoteTypeRequestSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));

        services.AddSingleton<ITimerSettingsCommandSender>(sp =>
            new TimerSettingsCommandSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));
        services.AddSingleton<ITimerSettingsRequestSender>(sp =>
            new TimerSettingsRequestSender(sp.GetRequiredService<IGrpcUrlService>().InternalToServerUrl));
    }
}