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
using Client.Desktop.Communication.Mock.Tracer;
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
using Client.Desktop.FileSystem;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Desktop.Presentation.Models.Mock;
using Client.Desktop.Presentation.Views.Mock;
using Client.Desktop.Services.Authentication;
using Client.Desktop.Services.Mock;
using Global.Settings;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using DebugWindow = Client.Desktop.Presentation.Views.Mock.DebugWindow;
using INoteTypeRequestSender = Client.Desktop.Communication.Requests.NoteType.INoteTypeRequestSender;
using ISprintRequestSender = Client.Desktop.Communication.Requests.Sprint.ISprintRequestSender;
using ITagRequestSender = Client.Desktop.Communication.Requests.Tag.ITagRequestSender;
using ITicketCommandSender = Client.Desktop.Communication.Commands.Ticket.ITicketCommandSender;
using ITicketRequestSender = Client.Desktop.Communication.Requests.Ticket.ITicketRequestSender;
using ITimerSettingsRequestSender = Client.Desktop.Communication.Requests.Timer.ITimerSettingsRequestSender;
using ITracingDataSender = Client.Tracing.ITracingDataSender;
using NoteTypeRequestSender = Client.Desktop.Communication.Requests.NoteType.NoteTypeRequestSender;
using SprintRequestSender = Client.Desktop.Communication.Requests.Sprint.SprintRequestSender;
using TagRequestSender = Client.Desktop.Communication.Requests.Tag.TagRequestSender;
using TicketCommandSender = Client.Desktop.Communication.Commands.Ticket.TicketCommandSender;
using TicketRequestSender = Client.Desktop.Communication.Requests.Ticket.TicketRequestSender;
using TimerSettingsRequestSender = Client.Desktop.Communication.Requests.Timer.TimerSettingsRequestSender;
using TracingDataSender = Client.Desktop.Communication.Commands.Tracing.TracingDataSender;

namespace Client.Desktop;

public static class CommunicationServices
{
    public static void AddCommunicationServices(this IServiceCollection services, bool isMock = false)
    {
        AddPolicyServices(services);
        services.AddSingleton<IStreamLifeCycleHandler, StreamLifeCycleHandler>();

        services.AddScoped<IMockSettingsService>(sp => new MockSettingsService(
            sp.GetRequiredService<IFileSystemWriter>(),
            sp.GetRequiredService<IFileSystemReader>(),
            sp.GetRequiredService<IFileSystemWrapper>()
        )
        {
            IsMockingModeActive = isMock
        });

        if (isMock)
        {
            AddMockTraceSender(services);
            AddMockSeedingServices(services);
            AddMockedClientServerServices(services);
            AddMockedRequestSenders(services);
            AddMockedServerServices(services);
            return;
        }

        AddTraceSender(services);
        AddNotificationReceivers(services);
        AddRequestSenders(services);
        AddCommandSenders(services);
    }

    private static void AddMockTraceSender(this IServiceCollection services)
    {
        services.AddScoped<MockLogger>();
        services.AddScoped<ITracingDataSender>(sp => sp.GetRequiredService<MockLogger>());
        services.AddScoped<IMockTraceDataPublisher>(sp => sp.GetRequiredService<MockLogger>());
    }

    private static void AddMockSeedingServices(this IServiceCollection services)
    {
        services.AddSingleton<IMockSeederFactory, MockSeederFactory>();
    }

    private static void AddMockedServerServices(this IServiceCollection services)
    {
        services.AddScoped<MockSeedSetupService>();
        services.AddScoped<IMockSeedSetupService>(sp => sp.GetRequiredService<MockSeedSetupService>());

        services.AddScoped<ServerLogModel>();
        services.AddScoped<IEventRegistration>(sp => sp.GetRequiredService<ServerLogModel>());
        services.AddScoped<ServerLogViewModel>();

        services.AddScoped<ServerSeedSettingsModel>();
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<ServerSeedSettingsModel>());
        services.AddScoped<ServerSeedSettingsViewModel>();

        services.AddScoped<MockDataService>();
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<MockDataService>());

        services.AddSingleton<Presentation.Models.Mock.DebugWindow>();

        services.AddSingleton<ServerNoteTypeDataViewModel>();
        services.AddSingleton<ServerNoteTypeDataModel>();

        services.AddSingleton<ServerSprintDataViewModel>();
        services.AddSingleton<ServerSprintDataModel>();

        services.AddSingleton<ServerTagDataViewModel>();
        services.AddSingleton<ServerTagDataModel>();

        services.AddSingleton<ServerTicketDataViewModel>();
        services.AddSingleton<ServerTicketDataModel>();

        services.AddSingleton<DebugWindow>();
        services.AddScoped<ServerNoteTypeDataControl>();
        services.AddScoped<ServerSprintsDataControl>();
        services.AddScoped<ServerTagsDataControl>();
        services.AddScoped<ServerTicketsDataControl>();
    }

    private static void AddMockedRequestSenders(this IServiceCollection services)
    {
        services.AddScoped<IRequestSender, RequestSender>();

        services.AddScoped<INotesRequestSender, MockNotesRequestSender>();
        services.AddScoped<ITimeSlotRequestSender, MockTimeSlotRequestSender>();
        services.AddScoped<IWorkDayRequestSender, MockWorkDayRequestSender>();
        services.AddScoped<ITicketReplayRequestSender, MockTicketReplayRequestSender>();
        services.AddScoped<IClientRequestSender, MockClientRequestSender>();
        services.AddScoped<IStatisticsDataRequestSender, MockStatisticsDataRequestSender>();
        services.AddScoped<IAnalysisRequestSender, MockAnalysisRequestSender>();
        services.AddScoped<ITimerSettingsRequestSender, MockTimerSettingsRequestSender>();

        services.AddScoped<IAnalysisMapper, AnalysisMapper>();
    }

    private static void AddMockedClientServerServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandSender, CommandSender>();
        services.AddScoped<INotificationPublisherFacade, NotificationPublisherFacade>();

        services.AddScoped<MockNoteCommandSender>();
        services.AddScoped<INoteCommandSender>(sp => sp.GetRequiredService<MockNoteCommandSender>());
        services.AddScoped<ILocalNoteNotificationPublisher>(sp => sp.GetRequiredService<MockNoteCommandSender>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<MockNoteCommandSender>());
        
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
        
        services.AddScoped<ITicketCommandSender>(sp => sp.GetRequiredService<ServerTicketDataModel>());
        services.AddScoped<ILocalTicketNotificationPublisher>(sp => sp.GetRequiredService<ServerTicketDataModel>());
        services.AddScoped<IStreamClient>(sp => sp.GetRequiredService<ServerTicketDataModel>());
        services.AddScoped<ITicketRequestSender>(sp => sp.GetRequiredService<ServerTicketDataModel>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<ServerTicketDataModel>());
    }

    private static void AddTraceSender(this IServiceCollection services)
    {
        services.AddScoped<TracingDataSender>(sp =>
            new TracingDataSender(sp.GetRequiredService<IGrpcUrlService>().ClientToMonitoringUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITracingDataSender>(sp => sp.GetRequiredService<TracingDataSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TracingDataSender>());
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
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<ClientNotificationReceiver>());

        services.AddSingleton<NoteNotificationReceiver>();
        services.AddSingleton<ILocalNoteNotificationPublisher>(sp =>
            sp.GetRequiredService<NoteNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<NoteNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<NoteNotificationReceiver>());

        services.AddSingleton<NoteTypeNotificationReceiver>();
        services.AddSingleton<ILocalNoteTypeNotificationPublisher>(sp =>
            sp.GetRequiredService<NoteTypeNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<NoteTypeNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<NoteTypeNotificationReceiver>());

        services.AddSingleton<SprintNotificationReceiver>();
        services.AddSingleton<ILocalSprintNotificationPublisher>(sp =>
            sp.GetRequiredService<SprintNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<SprintNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<SprintNotificationReceiver>());

        services.AddSingleton<StatisticsDataNotificationReceiver>();
        services.AddSingleton<ILocalStatisticsDataNotificationPublisher>(sp =>
            sp.GetRequiredService<StatisticsDataNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<StatisticsDataNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<StatisticsDataNotificationReceiver>());

        services.AddSingleton<TagNotificationReceiver>();
        services.AddSingleton<ILocalTagNotificationPublisher>(sp => sp.GetRequiredService<TagNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<TagNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TagNotificationReceiver>());

        services.AddSingleton<TicketNotificationReceiver>();
        services.AddSingleton<ILocalTicketNotificationPublisher>(sp =>
            sp.GetRequiredService<TicketNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<TicketNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TicketNotificationReceiver>());

        services.AddSingleton<TimerSettingsNotificationReceiver>();
        services.AddSingleton<ILocalTimerSettingsNotificationPublisher>(sp =>
            sp.GetRequiredService<TimerSettingsNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<TimerSettingsNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TimerSettingsNotificationReceiver>());

        services.AddSingleton<WorkDayNotificationReceiver>();
        services.AddSingleton<ILocalWorkDayNotificationPublisher>(sp =>
            sp.GetRequiredService<WorkDayNotificationReceiver>());
        services.AddSingleton<IStreamClient>(sp => sp.GetRequiredService<WorkDayNotificationReceiver>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<WorkDayNotificationReceiver>());

        services.AddSingleton<INotificationPublisherFacade, NotificationPublisherFacade>();
    }

    private static void AddCommandSenders(this IServiceCollection services)
    {
        services.AddScoped<ICommandSender, CommandSender>();

        services.AddScoped<TicketCommandSender>(sp =>
            new TicketCommandSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITicketCommandSender>(sp => sp.GetRequiredService<TicketCommandSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TicketCommandSender>());

        services.AddScoped<NoteCommandSender>(sp =>
            new NoteCommandSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<INoteCommandSender>(sp => sp.GetRequiredService<NoteCommandSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<NoteCommandSender>());

        services.AddScoped<StatisticsDataCommandSender>(sp =>
            new StatisticsDataCommandSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<IStatisticsDataCommandSender>(sp => sp.GetRequiredService<StatisticsDataCommandSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<StatisticsDataCommandSender>());

        services.AddScoped<TimeSlotCommandSender>(sp =>
            new TimeSlotCommandSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITimeSlotCommandSender>(sp => sp.GetRequiredService<TimeSlotCommandSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TimeSlotCommandSender>());

        services.AddScoped<ClientCommandSender>(sp => new ClientCommandSender(
            sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
            sp.GetRequiredService<ITokenService>()));
        services.AddScoped<IClientCommandSender>(sp => sp.GetRequiredService<ClientCommandSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<ClientCommandSender>());

        services.AddScoped<WorkDayCommandSender>(sp =>
            new WorkDayCommandSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<IWorkDayCommandSender>(sp => sp.GetRequiredService<WorkDayCommandSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<WorkDayCommandSender>());
    }

    private static void AddRequestSenders(this IServiceCollection services)
    {
        services.AddScoped<IRequestSender, RequestSender>();

        services.AddScoped<TicketRequestSender>(sp =>
            new TicketRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITicketRequestSender>(sp => sp.GetRequiredService<TicketRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TicketRequestSender>());

        services.AddScoped<SprintRequestSender>(sp =>
            new SprintRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ISprintRequestSender>(sp => sp.GetRequiredService<SprintRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<SprintRequestSender>());

        services.AddScoped<TagRequestSender>(sp =>
            new TagRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITagRequestSender>(sp => sp.GetRequiredService<TagRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TagRequestSender>());

        services.AddScoped<NoteTypeRequestSender>(sp =>
            new NoteTypeRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<INoteTypeRequestSender>(sp => sp.GetRequiredService<NoteTypeRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<NoteTypeRequestSender>());

        services.AddScoped<TimerSettingsRequestSender>(sp =>
            new TimerSettingsRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITimerSettingsRequestSender>(sp => sp.GetRequiredService<TimerSettingsRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TimerSettingsRequestSender>());

        services.AddScoped<NotesRequestSender>(sp =>
            new NotesRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<INotesRequestSender>(sp => sp.GetRequiredService<NotesRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<NotesRequestSender>());

        services.AddScoped<TimeSlotRequestSender>(sp =>
            new TimeSlotRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITimeSlotRequestSender>(sp => sp.GetRequiredService<TimeSlotRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TimeSlotRequestSender>());

        services.AddScoped<WorkDayRequestSender>(sp =>
            new WorkDayRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<IWorkDayRequestSender>(sp => sp.GetRequiredService<WorkDayRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<WorkDayRequestSender>());

        services.AddScoped<TicketReplayRequestSender>(sp =>
            new TicketReplayRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<ITicketReplayRequestSender>(sp => sp.GetRequiredService<TicketReplayRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<TicketReplayRequestSender>());

        services.AddScoped<ClientRequestSender>(sp =>
            new ClientRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<IClientRequestSender>(sp => sp.GetRequiredService<ClientRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<ClientRequestSender>());

        services.AddScoped<StatisticsDataRequestSender>(sp =>
            new StatisticsDataRequestSender(sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl,
                sp.GetRequiredService<ITokenService>()));
        services.AddScoped<IStatisticsDataRequestSender>(sp => sp.GetRequiredService<StatisticsDataRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<StatisticsDataRequestSender>());

        services.AddScoped<AnalysisRequestSender>(sp =>
            new AnalysisRequestSender(sp.GetRequiredService<IAnalysisMapper>(),
                sp.GetRequiredService<IGrpcUrlService>().ClientToServerUrl, sp.GetRequiredService<ITokenService>()));
        services.AddScoped<IAnalysisRequestSender>(sp => sp.GetRequiredService<AnalysisRequestSender>());
        services.AddScoped<IInitializeAsync>(sp => sp.GetRequiredService<AnalysisRequestSender>());

        services.AddScoped<IAnalysisMapper, AnalysisMapper>();
    }
}