using System;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Client;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.WorkDays;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Mock;
using Client.Desktop.Communication.Mock.Commands;
using Client.Desktop.Communication.Mock.Requests;
using Client.Desktop.Communication.Notifications.Client.Receiver;
using Client.Desktop.Communication.Notifications.Note.Receiver;
using Client.Desktop.Communication.Notifications.NoteType.Receiver;
using Client.Desktop.Communication.Notifications.Sprint.Receiver;
using Client.Desktop.Communication.Notifications.StatisticsData.Receiver;
using Client.Desktop.Communication.Notifications.Tag.Receiver;
using Client.Desktop.Communication.Notifications.Ticket.Receiver;
using Client.Desktop.Communication.Notifications.TimerSettings.Receiver;
using Client.Desktop.Communication.Notifications.WorkDay.Receiver;
using Client.Desktop.Communication.Policies;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Client;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Global.Settings.UrlResolver;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Service.Monitoring.Shared.Tracing;
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

namespace Client.Desktop;

public static class CommunicationServices
{
    public static void AddCommunicationServices(this IServiceCollection services, bool isMock = false)
    {
        AddPolicyServices(services);
        services.AddSingleton<IStreamLifeCycleHandler, StreamLifeCycleHandler>();
        
        if (isMock)
        {
            services.AddScoped<MockDataService>();
            AddMockedCommandSendersWithPublishers(services);
            AddMockedRequestSenders(services);
            return;
        }

        AddTraceSender(services);
        AddSharedDataServices(services);
        AddNotificationReceivers(services);
        AddRequestSenders(services);
        AddCommandSenders(services);
    }

    private static void AddMockedRequestSenders(this IServiceCollection services)
    {
        services.AddScoped<IRequestSender, RequestSender>();
        
        services.AddScoped<INotesRequestSender, MockNotesRequestSender>();
        services.AddScoped<ISprintRequestSender, MockSprintRequestSender>();
        services.AddScoped<INoteTypeRequestSender, MockNoteTypeRequestSender>();
        services.AddScoped<ITimeSlotRequestSender, MockTimeSlotRequestSender>();
        services.AddScoped<IWorkDayRequestSender, MockWorkDayRequestSender>();
        services.AddScoped<ITicketReplayRequestSender, MockTicketReplayRequestSender>();
        services.AddScoped<ITicketRequestSender, MockTicketRequestSender>();
        services.AddScoped<IClientRequestSender, MockClientRequestSender>();
        services.AddScoped<IStatisticsDataRequestSender, MockStatisticsDataRequestSender>();
        services.AddScoped<IAnalysisRequestSender, MockAnalysisRequestSender>();
        services.AddScoped<ITagRequestSender, MockTagRequestSender>();
        services.AddScoped<ITimerSettingsRequestSender, MockTimerSettingsRequestSender>();
        
        services.AddScoped<IAnalysisMapper, AnalysisMapper>();
    }

    private static void AddMockedCommandSendersWithPublishers(this IServiceCollection services)
    {
        services.AddScoped<ICommandSender, CommandSender>();
        services.AddScoped<INotificationPublisherFacade, NotificationPublisherFacade>();

        services.AddScoped<MockNoteCommandSender>();
        services.AddScoped<INoteCommandSender>(sp => sp.GetRequiredService<MockNoteCommandSender>());
        services.AddScoped<ILocalNoteNotificationPublisher>(sp => sp.GetRequiredService<MockNoteCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockNoteCommandSender>());
        
        services.AddScoped<MockNoteTypeCommandSender>();
        services.AddScoped<INoteTypeCommandSender>(sp => sp.GetRequiredService<MockNoteTypeCommandSender>());
        services.AddScoped<ILocalNoteTypeNotificationPublisher>(sp => sp.GetRequiredService<MockNoteTypeCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockNoteTypeCommandSender>());

        services.AddScoped<MockSprintCommandSender>();
        services.AddScoped<ISprintCommandSender>(sp => sp.GetRequiredService<MockSprintCommandSender>());
        services.AddScoped<ILocalSprintNotificationPublisher>(sp => sp.GetRequiredService<MockSprintCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockSprintCommandSender>());
        
        services.AddScoped<MockTagCommandSender>();
        services.AddScoped<ITagCommandSender>(sp => sp.GetRequiredService<MockTagCommandSender>());
        services.AddScoped<ILocalTagNotificationPublisher>(sp => sp.GetRequiredService<MockTagCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockTagCommandSender>());
        
        services.AddScoped<MockTimerSettingsCommandSender>();
        services.AddScoped<ITimerSettingsCommandSender>(sp => sp.GetRequiredService<MockTimerSettingsCommandSender>());
        services.AddScoped<ILocalTimerSettingsNotificationPublisher>(sp => sp.GetRequiredService<MockTimerSettingsCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockTimerSettingsCommandSender>());
        
        services.AddScoped<MockStatisticsDataCommandSender>();
        services.AddScoped<IStatisticsDataCommandSender>(sp =>
            sp.GetRequiredService<MockStatisticsDataCommandSender>());
        services.AddScoped<ILocalStatisticsDataNotificationPublisher>(sp =>
            sp.GetRequiredService<MockStatisticsDataCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockStatisticsDataCommandSender>());
        
        services.AddScoped<MockTimeSlotCommandSender>();
        services.AddScoped<ITimeSlotCommandSender>(sp => sp.GetRequiredService<MockTimeSlotCommandSender>());
        
        services.AddScoped<MockClientCommandSender>();
        services.AddScoped<IClientCommandSender>(sp => sp.GetRequiredService<MockClientCommandSender>());
        services.AddScoped<ILocalClientNotificationPublisher>(sp => sp.GetRequiredService<MockClientCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockClientCommandSender>());

        services.AddScoped<MockWorkDayCommandSender>();
        services.AddScoped<IWorkDayCommandSender>(sp => sp.GetRequiredService<MockWorkDayCommandSender>());
        services.AddScoped<ILocalWorkDayNotificationPublisher>(sp => sp.GetRequiredService<MockWorkDayCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockWorkDayCommandSender>());
        
        services.AddScoped<MockTicketCommandSender>();
        services.AddScoped<ITicketCommandSender>(sp => sp.GetRequiredService<MockTicketCommandSender>());
        services.AddScoped<ILocalTicketNotificationPublisher>(sp => sp.GetRequiredService<MockTicketCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockTicketCommandSender>());
    }
    
    private static void AddTraceSender(this IServiceCollection services)
    {
        services.AddScoped<ITracingDataSender>(sp =>
            new TracingDataSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Monitoring)
                .BuildAddress()));
    }

    private static void AddSharedDataServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketCommandSender>(sp =>
            new TicketCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<ITicketRequestSender>(sp =>
            new TicketRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));

        services.AddScoped<ISprintCommandSender>(sp =>
            new SprintCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<ISprintRequestSender>(sp =>
            new SprintRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));

        services.AddScoped<ITagCommandSender>(sp =>
            new TagCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<ITagRequestSender>(sp =>
            new TagRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));

        services.AddScoped<INoteTypeCommandSender>(sp =>
            new NoteTypeCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<INoteTypeRequestSender>(sp =>
            new NoteTypeRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));

        services.AddScoped<ITimerSettingsRequestSender>(sp =>
            new TimerSettingsRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
    }

    private static void AddPolicyServices(IServiceCollection services)
    {
        services.AddSingleton(
            new CommandRetryPolicy(Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .WrapAsync(Policy
                    .Handle<Exception>()
                    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30))))
        );

        services.AddSingleton(
            new RequestRetryPolicy(Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .WrapAsync(Policy
                    .Handle<Exception>()
                    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30))))
        );
    }

    private static void AddNotificationReceivers(this IServiceCollection services)
    {
        services.AddSingleton<ClientNotificationReceiver>();
        services.AddSingleton<ILocalClientNotificationPublisher>(sp =>
            sp.GetRequiredService<ClientNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<ClientNotificationReceiver>());

        services.AddSingleton<NoteNotificationReceiver>();
        services.AddSingleton<ILocalNoteNotificationPublisher>(sp =>
            sp.GetRequiredService<NoteNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<NoteNotificationReceiver>());

        services.AddSingleton<NoteTypeNotificationReceiver>();
        services.AddSingleton<ILocalNoteTypeNotificationPublisher>(sp =>
            sp.GetRequiredService<NoteTypeNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<NoteTypeNotificationReceiver>());

        services.AddSingleton<SprintNotificationReceiver>();
        services.AddSingleton<ILocalSprintNotificationPublisher>(sp =>
            sp.GetRequiredService<SprintNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<SprintNotificationReceiver>());

        services.AddSingleton<StatisticsDataNotificationReceiver>();
        services.AddSingleton<ILocalStatisticsDataNotificationPublisher>(sp =>
            sp.GetRequiredService<StatisticsDataNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<StatisticsDataNotificationReceiver>());

        services.AddSingleton<TagNotificationReceiver>();
        services.AddSingleton<ILocalTagNotificationPublisher>(sp => sp.GetRequiredService<TagNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<TagNotificationReceiver>());

        services.AddSingleton<TicketNotificationReceiver>();
        services.AddSingleton<ILocalTicketNotificationPublisher>(sp =>
            sp.GetRequiredService<TicketNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<TicketNotificationReceiver>());

        services.AddSingleton<TimerSettingsNotificationReceiver>();
        services.AddSingleton<ILocalTimerSettingsNotificationPublisher>(sp =>
            sp.GetRequiredService<TimerSettingsNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<TimerSettingsNotificationReceiver>());

        services.AddSingleton<WorkDayNotificationReceiver>();
        services.AddSingleton<ILocalWorkDayNotificationPublisher>(sp =>
            sp.GetRequiredService<WorkDayNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<WorkDayNotificationReceiver>());

        services.AddSingleton<INotificationPublisherFacade, NotificationPublisherFacade>();
    }

    private static void AddCommandSenders(this IServiceCollection services)
    {
        services.AddScoped<ICommandSender, CommandSender>();

        services.AddScoped<INoteCommandSender>(sp =>
            new NoteCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<IStatisticsDataCommandSender>(sp =>
            new StatisticsDataCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<ITimeSlotCommandSender>(sp =>
            new TimeSlotCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<IClientCommandSender>(sp =>
            new ClientCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<IWorkDayCommandSender>(sp =>
            new WorkDayCommandSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
    }

    private static void AddRequestSenders(this IServiceCollection services)
    {
        services.AddScoped<IRequestSender, RequestSender>();

        services.AddScoped<INotesRequestSender>(sp =>
            new NotesRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<ITimeSlotRequestSender>(sp =>
            new TimeSlotRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<IWorkDayRequestSender>(sp =>
            new WorkDayRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<ITicketReplayRequestSender>(sp =>
            new TicketReplayRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<IClientRequestSender>(sp =>
            new ClientRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<IStatisticsDataRequestSender>(sp =>
            new StatisticsDataRequestSender(sp.GetRequiredService<IGrpcUrlBuilder>()
                .From(ResolvingServices.Client)
                .To(ResolvingServices.Server)
                .BuildAddress()));
        services.AddScoped<IAnalysisRequestSender>(sp =>
            new AnalysisRequestSender(sp.GetRequiredService<IAnalysisMapper>(),
                sp.GetRequiredService<IGrpcUrlBuilder>()
                    .From(ResolvingServices.Client)
                    .To(ResolvingServices.Server)
                    .BuildAddress()));

        services.AddScoped<IAnalysisMapper, AnalysisMapper>();
    }
}