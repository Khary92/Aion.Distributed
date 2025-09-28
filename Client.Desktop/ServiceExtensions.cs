using System;
using System.Threading;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Client;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Commands.WorkDays;
using Client.Desktop.Communication.Notifications.Client;
using Client.Desktop.Communication.Notifications.Note;
using Client.Desktop.Communication.Notifications.NoteType;
using Client.Desktop.Communication.Notifications.Sprint;
using Client.Desktop.Communication.Notifications.StatisticsData;
using Client.Desktop.Communication.Notifications.Tag;
using Client.Desktop.Communication.Notifications.Ticket;
using Client.Desktop.Communication.Notifications.TimerSettings;
using Client.Desktop.Communication.Notifications.WorkDay;
using Client.Desktop.Communication.Policies;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Client;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.Config;
using Client.Desktop.FileSystem;
using Client.Desktop.FileSystem.Serializer;
using Client.Desktop.Lifecycle.Shutdown;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Lifecycle.Startup.Tasks.Streams;
using Client.Desktop.Lifecycle.Startup.Tasks.UnsentCommands;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Desktop.Presentation.Models.Documentation;
using Client.Desktop.Presentation.Models.Export;
using Client.Desktop.Presentation.Models.Main;
using Client.Desktop.Presentation.Models.Settings;
using Client.Desktop.Presentation.Models.Synchronization;
using Client.Desktop.Presentation.Models.TimeTracking;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Client.Desktop.Presentation.Views.Analysis;
using Client.Desktop.Presentation.Views.Documentation;
using Client.Desktop.Presentation.Views.Export;
using Client.Desktop.Presentation.Views.Main;
using Client.Desktop.Presentation.Views.Setting;
using Client.Desktop.Presentation.Views.Tracking;
using Client.Desktop.Services;
using Client.Desktop.Services.Cache;
using Client.Desktop.Services.Export;
using Client.Desktop.Services.LocalSettings;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Proto.Notifications.Client;
using Proto.Notifications.Note;
using Proto.Notifications.NoteType;
using Proto.Notifications.Sprint;
using Proto.Notifications.StatisticsData;
using Proto.Notifications.Tag;
using Proto.Notifications.Ticket;
using Proto.Notifications.TimerSettings;
using Proto.Notifications.WorkDay;
using Service.Proto.Shared.Commands.NoteTypes;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tags;
using Service.Proto.Shared.Commands.Tickets;
using Service.Proto.Shared.Requests.NoteTypes;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tags;
using Service.Proto.Shared.Requests.Tickets;
using Service.Proto.Shared.Requests.TimerSettings;

namespace Client.Desktop;

public static class ServiceExtensions
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        AddConfigServices(services);
        AddPolicyServices(services);
        AddSchedulerServices(services);
        AddLocalServices(services);
        AddSharedDataServices(services);
        AddSynchronizationServices(services);
        AddViews(services);
        AddModels(services);
        AddViewFactories(services);
        AddNotificationReceivers(services);
        AddRequestSenders(services);
        AddCommandSenders(services);
        AddFileSystemServices(services);
    }

    private static void AddConfigServices(IServiceCollection services)
    {
        services.AddSingleton<IClientConfig, ClientConfig>();
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

    private static void AddSchedulerServices(this IServiceCollection services)
    {
        services.AddSingleton<CancellationTokenSource>();
        services.AddSingleton<IStartupScheduler, StartupScheduler>();

        services.AddSingleton<IStartupTask, AsyncInitializeTask>();
        services.AddSingleton<IStartupTask, RegisterMessengerTask>();
        services.AddSingleton<IStartupTask, SendUnsentCommandsTask>();

        services.AddSingleton<StreamLifeCycleHandler>();
        services.AddSingleton<IStreamLifeCycleHandler>(sp => sp.GetRequiredService<StreamLifeCycleHandler>());

        services.AddSingleton<IShutDownHandler, ShutdownHandler>();
    }

    private static void AddLocalServices(this IServiceCollection services)
    {
        services.AddSingleton<LocalSettingsProjector>();
        services.AddSingleton<ILocalSettingsService>(sp => sp.GetRequiredService<LocalSettingsProjector>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<LocalSettingsProjector>());
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<LocalSettingsProjector>());

        services.AddSingleton<ILocalSettingsCommandSender, LocalSettingsCommandSender>();

        services.AddSingleton<ExportService>();
        services.AddSingleton<IExportService>(sp => sp.GetRequiredService<ExportService>());

        services.AddSingleton<TimerService>();
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TimerService>());
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<TimerService>());

        services.AddSingleton<DocumentationSynchronizer>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<DocumentationSynchronizer>());
        services.AddSingleton<IDocumentationSynchronizer>(sp => sp.GetRequiredService<DocumentationSynchronizer>());
    }

    private static void AddSharedDataServices(this IServiceCollection services)
    {
        services.AddScoped<ITicketCommandSender>(sp =>
            new TicketCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<ITicketRequestSender>(sp =>
            new TicketRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));

        services.AddScoped<ISprintCommandSender>(sp =>
            new SprintCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<ISprintRequestSender>(sp =>
            new SprintRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));

        services.AddScoped<ITagCommandSender>(sp =>
            new TagCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<ITagRequestSender>(sp =>
            new TagRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));

        services.AddScoped<INoteTypeCommandSender>(sp =>
            new NoteTypeCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<INoteTypeRequestSender>(sp =>
            new NoteTypeRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));

        services.AddScoped<ITimerSettingsRequestSender>(sp =>
            new TimerSettingsRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
    }

    private static void AddFileSystemServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileSystemReader, JsonReader>();
        services.AddSingleton<IFileSystemWriter, FileSystemWriter>();
        services.AddSingleton<IFileSystemWrapper, FileSystemWrapper>();
    }

    private static void AddSynchronizationServices(this IServiceCollection services)
    {
        services.AddSingleton<IPersistentCache<ClientSetStartTimeCommand>, StartTimeChangedCache>();
        services.AddSingleton<IPersistentCache<ClientSetEndTimeCommand>, EndTimeChangedCache>();
    }

    private static void AddViews(this IServiceCollection services)
    {
        services.AddSingleton<ContentWrapper>();
        services.AddSingleton<SettingsCompositeControl>();
        services.AddSingleton<TrackingWrapperControl>();
        services.AddSingleton<DocumentationControl>();
        services.AddSingleton<ExportControl>();
        services.AddSingleton<AnalysisControlWrapper>();
    }

    private static void AddModels(this IServiceCollection services)
    {
        services.AddTransient<TypeCheckBoxViewModel>();
        services.AddTransient<TrackingSlotViewModel>();
        services.AddTransient<TrackingSlotModel>();

        services.AddSingleton<AnalysisByTagViewModel>();
        services.AddSingleton<AnalysisByTicketViewModel>();
        services.AddSingleton<AnalysisBySprintViewModel>();

        services.AddSingleton<AnalysisControlWrapperViewModel>();
        services.AddSingleton<ContentWrapperViewModel>();
        services.AddSingleton<SettingsCompositeViewModel>();

        services.AddSingleton<WorkDaysViewModel>();
        services.AddSingleton<TimeTrackingViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<ExportViewModel>();

        services.AddSingleton<DocumentationViewModel>();
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<DocumentationViewModel>());

        services.AddSingleton<DocumentationModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<DocumentationModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<DocumentationModel>());

        services.AddSingleton<SettingsModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<SettingsModel>());

        services.AddSingleton<WorkDaysModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<WorkDaysModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<WorkDaysModel>());

        services.AddSingleton<TimeTrackingModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<TimeTrackingModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TimeTrackingModel>());

        services.AddSingleton<ExportModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<ExportModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<ExportModel>());

        services.AddSingleton<AnalysisByTagModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<AnalysisByTagModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<AnalysisByTagModel>());

        services.AddSingleton<AnalysisByTicketModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<AnalysisByTicketModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<AnalysisByTicketModel>());

        services.AddSingleton<AnalysisBySprintModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<AnalysisBySprintModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<AnalysisBySprintModel>());
    }

    private static void AddViewFactories(this IServiceCollection services)
    {
        services.AddTransient<INoteStreamViewModelFactory, NoteStreamViewModelFactory>();
        services.AddTransient<NoteStreamViewModel>();

        services.AddTransient<ITypeCheckBoxViewModelFactory, TypeCheckBoxViewModelFactory>();
        services.AddTransient<NoteStreamViewModel>();

        services.AddTransient<INoteViewFactory, NoteViewFactory>();
        services.AddTransient<NoteViewModel>();

        services.AddTransient<ITagCheckBoxViewFactory, TagCheckBoxViewFactory>();
        services.AddTransient<TagCheckBoxViewModel>();

        services.AddSingleton<IStatisticsViewModelFactory, StatisticsViewModelFactory>();
        services.AddTransient<StatisticsViewModel>();

        services.AddSingleton<ITrackingSlotViewModelFactory, TrackingSlotViewModelFactory>();
    }

    private static void AddNotificationReceivers(this IServiceCollection services)
    {
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        services.AddSingleton<IStreamClient, TicketNotificationReceiver>();
        services.AddSingleton<IStreamClient, NoteNotificationStream>();
        services.AddSingleton<IStreamClient, NoteTypeNotificationReceiver>();
        services.AddSingleton<IStreamClient, SprintNotificationReceiver>();
        services.AddSingleton<IStreamClient, TagNotificationReceiver>();
        services.AddSingleton<IStreamClient, ClientNotificationReceiver>();
        services.AddSingleton<IStreamClient, WorkDayNotificationReceiver>();
        services.AddSingleton<IStreamClient, TimerSettingsNotificationReceiver>();
        services.AddSingleton<IStreamClient, StatisticsDataNotificationReceiver>();

        services.AddSingleton<GrpcChannel>(sp =>
            GrpcChannel.ForAddress(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddSingleton<TicketNotificationService.TicketNotificationServiceClient>();
        services.AddSingleton<NoteNotificationService.NoteNotificationServiceClient>();
        services.AddSingleton<SprintNotificationService.SprintNotificationServiceClient>();
        services.AddSingleton<TagNotificationService.TagNotificationServiceClient>();
        services.AddSingleton<TimerSettingsNotificationService.TimerSettingsNotificationServiceClient>();
        services.AddSingleton<ClientNotificationService.ClientNotificationServiceClient>();
        services.AddSingleton<WorkDayNotificationService.WorkDayNotificationServiceClient>();
        services.AddSingleton<NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient>();
        services.AddSingleton<StatisticsDataNotificationService.StatisticsDataNotificationServiceClient>();
    }

    private static void AddCommandSenders(this IServiceCollection services)
    {
        services.AddScoped<ICommandSender, CommandSender>();

        services.AddScoped<INoteCommandSender>(sp =>
            new NoteCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<IStatisticsDataCommandSender>(sp =>
            new StatisticsDataCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<ITimeSlotCommandSender>(sp =>
            new TimeSlotCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<IClientCommandSender>(sp =>
            new ClientCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<IWorkDayCommandSender>(sp =>
            new WorkDayCommandSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
    }

    private static void AddRequestSenders(this IServiceCollection services)
    {
        services.AddScoped<IRequestSender, RequestSender>();

        services.AddScoped<INotesRequestSender>(sp =>
            new NotesRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<ITimeSlotRequestSender>(sp =>
            new TimeSlotRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<IWorkDayRequestSender>(sp =>
            new WorkDayRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<ITicketReplayRequestSender>(sp =>
            new TicketReplayRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<IClientRequestSender>(sp =>
            new ClientRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<IStatisticsDataRequestSender>(sp =>
            new StatisticsDataRequestSender(sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));
        services.AddScoped<IAnalysisRequestSender>(sp =>
            new AnalysisRequestSender(sp.GetRequiredService<IAnalysisMapper>(),
                sp.GetRequiredService<IClientConfig>().GetCoreServerUrl()));

        services.AddScoped<IAnalysisMapper, AnalysisMapper>();
    }
}