using System;
using System.Threading;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.WorkDays;
using Client.Desktop.Communication.Notifications.Note;
using Client.Desktop.Communication.Notifications.NoteType;
using Client.Desktop.Communication.Notifications.Sprint;
using Client.Desktop.Communication.Notifications.StatisticsData;
using Client.Desktop.Communication.Notifications.Tag;
using Client.Desktop.Communication.Notifications.Ticket;
using Client.Desktop.Communication.Notifications.TimerSettings;
using Client.Desktop.Communication.Notifications.UseCase;
using Client.Desktop.Communication.Notifications.WorkDay;
using Client.Desktop.Communication.Policies;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.UseCase;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.DataModels.Decorators.Replays;
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
using Client.Proto;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Proto.Notifications.Note;
using Proto.Notifications.NoteType;
using Proto.Notifications.Sprint;
using Proto.Notifications.StatisticsData;
using Proto.Notifications.Tag;
using Proto.Notifications.Ticket;
using Proto.Notifications.TimerSettings;
using Proto.Notifications.UseCase;
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
    }

    private static void AddSharedDataServices(this IServiceCollection services)
    {
        var serverAddress = "http://localhost:8081";
        services.AddScoped<ITicketCommandSender>(_ => new TicketCommandSender(serverAddress));
        services.AddScoped<ITicketRequestSender>(_ => new TicketRequestSender(serverAddress));

        services.AddScoped<ISprintCommandSender>(_ => new SprintCommandSender(serverAddress));
        services.AddScoped<ISprintRequestSender>(_ => new SprintRequestSender(serverAddress));

        services.AddScoped<ITagCommandSender>(_ => new TagCommandSender(serverAddress));
        services.AddScoped<ITagRequestSender>(_ => new TagRequestSender(serverAddress));

        services.AddScoped<INoteTypeCommandSender>(_ => new NoteTypeCommandSender(serverAddress));
        services.AddScoped<INoteTypeRequestSender>(_ => new NoteTypeRequestSender(serverAddress));

        services.AddScoped<ITimerSettingsRequestSender>(_ => new TimerSettingsRequestSender(serverAddress));
    }

    private static void AddFileSystemServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileSystemReader, JsonReader>();
        services.AddSingleton<IFileSystemWriter, FileSystemWriter>();
        services.AddSingleton<IFileSystemWrapper, FileSystemWrapper>();
    }

    private static void AddSynchronizationServices(this IServiceCollection services)
    {
        services.AddSingleton<IStateSynchronizer<TicketReplayDecorator, string>, DocumentationSynchronizer>();
        services.AddSingleton<IPersistentCache<ClientSetStartTimeCommand>, StartTimeChangedCache>();
        services.AddSingleton<IPersistentCache<ClientSetEndTimeCommand>, EndTimeChangedCache>();
    }

    private static void AddViews(this IServiceCollection services)
    {
        services.AddSingleton<ContentWrapper>();
        services.AddSingleton<SettingsCompositeControl>();
        services.AddSingleton<TimeTrackingControl>();
        services.AddSingleton<DocumentationControl>();
        services.AddSingleton<ExportControl>();
        services.AddSingleton<AnalysisControlWrapper>();
    }

    private static void AddModels(this IServiceCollection services)
    {
        services.AddTransient<TypeCheckBoxViewModel>();
        services.AddTransient<TimeSlotViewModel>();
        services.AddTransient<TimeSlotModel>();

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

        services.AddSingleton<ITimeSlotViewModelFactory, TimeSlotViewModelFactory>();
    }

    private static void AddNotificationReceivers(this IServiceCollection services)
    {
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        services.AddSingleton<IStreamClient, TicketNotificationReceiver>();
        services.AddSingleton<IStreamClient, NoteNotificationStream>();
        services.AddSingleton<IStreamClient, NoteTypeNotificationReceiver>();
        services.AddSingleton<IStreamClient, SprintNotificationReceiver>();
        services.AddSingleton<IStreamClient, TagNotificationReceiver>();
        services.AddSingleton<IStreamClient, UseCaseNotificationReceiver>();
        services.AddSingleton<IStreamClient, WorkDayNotificationReceiver>();
        services.AddSingleton<IStreamClient, TimerSettingsNotificationReceiver>();
        services.AddSingleton<IStreamClient, StatisticsDataNotificationReceiver>();

        var channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
        services.AddSingleton(new TicketNotificationService.TicketNotificationServiceClient(channel));
        services.AddSingleton(new NoteNotificationService.NoteNotificationServiceClient(channel));
        services.AddSingleton(new SprintNotificationService.SprintNotificationServiceClient(channel));
        services.AddSingleton(new TagNotificationService.TagNotificationServiceClient(channel));
        services.AddSingleton(new TimerSettingsNotificationService.TimerSettingsNotificationServiceClient(channel));
        services.AddSingleton(new UseCaseNotificationService.UseCaseNotificationServiceClient(channel));
        services.AddSingleton(new WorkDayNotificationService.WorkDayNotificationServiceClient(channel));
        services.AddSingleton(new NoteTypeProtoNotificationService.NoteTypeProtoNotificationServiceClient(channel));
        services.AddSingleton(new StatisticsDataNotificationService.StatisticsDataNotificationServiceClient(channel));
    }

    private static void AddCommandSenders(this IServiceCollection services)
    {
        services.AddScoped<ICommandSender, CommandSender>();

        services.AddScoped<INoteCommandSender, NoteCommandSender>();
        services.AddScoped<IStatisticsDataCommandSender, StatisticsDataCommandSender>();
        services.AddScoped<ITimeSlotCommandSender, TimeSlotCommandSender>();
        services.AddScoped<IUseCaseCommandSender, UseCaseCommandSender>();
        services.AddScoped<IWorkDayCommandSender, WorkDayCommandSender>();
    }

    private static void AddRequestSenders(this IServiceCollection services)
    {
        services.AddScoped<IRequestSender, RequestSender>();

        services.AddScoped<INotesRequestSender, NotesRequestSender>();
        services.AddScoped<ITimeSlotRequestSender, TimeSlotRequestSender>();
        services.AddScoped<IWorkDayRequestSender, WorkDayRequestSender>();
        services.AddScoped<ITicketReplayRequestSender, TicketReplayRequestSender>();
        services.AddScoped<IUseCaseRequestSender, UseCaseRequestSender>();
        services.AddScoped<IStatisticsDataRequestSender, StatisticsDataRequestSender>();

        services.AddScoped<IAnalysisRequestSender, AnalysisRequestSender>();
        services.AddScoped<IAnalysisMapper, AnalysisMapper>();
    }
}