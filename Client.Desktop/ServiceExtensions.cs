using System;
using System.Threading;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.Communication.Local.LocalEvents.Publisher;
using Client.Desktop.FileSystem;
using Client.Desktop.FileSystem.Serializer;
using Client.Desktop.Lifecycle.Shutdown;
using Client.Desktop.Lifecycle.Startup.Scheduler;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Desktop.Lifecycle.Startup.Tasks.UnsentCommands;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Presentation.Models.Analysis;
using Client.Desktop.Presentation.Models.Documentation;
using Client.Desktop.Presentation.Models.Export;
using Client.Desktop.Presentation.Models.Main;
using Client.Desktop.Presentation.Models.Replay;
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
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop;

public static class ServiceExtensions
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        AddSchedulerServices(services);
        AddLocalServices(services);
        AddSynchronizationServices(services);
        AddViews(services);
        AddModels(services);
        AddViewFactories(services);
        AddFileSystemServices(services);
    }

    private static void AddSchedulerServices(this IServiceCollection services)
    {
        services.AddSingleton<CancellationTokenSource>();
        services.AddSingleton<IStartupScheduler, StartupScheduler>();

        services.AddSingleton<IStartupTask, AsyncInitializeTask>();
        services.AddSingleton<IStartupTask, RegisterMessengerTask>();
        services.AddSingleton<IStartupTask, SendUnsentCommandsTask>();

        services.AddSingleton<IShutDownHandler, ShutdownHandler>();
    }

    private static void AddLocalServices(this IServiceCollection services)
    {
        services.AddSingleton<LocalSettingsService>();
        services.AddSingleton<ILocalSettingsService>(sp => sp.GetRequiredService<LocalSettingsService>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<LocalSettingsService>());

        services.AddSingleton<ExportService>();
        services.AddSingleton<IExportService>(sp => sp.GetRequiredService<ExportService>());

        services.AddSingleton<TimerService>();
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<TimerService>());
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<TimerService>());
        services.AddSingleton<IDisposable>(sp => sp.GetRequiredService<TimerService>());
        services.AddSingleton<IClientTimerNotificationPublisher>(sp => sp.GetRequiredService<TimerService>());

        services.AddSingleton<DocumentationSynchronizer>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<DocumentationSynchronizer>());
        services.AddSingleton<IDocumentationSynchronizer>(sp => sp.GetRequiredService<DocumentationSynchronizer>());

        services.AddTransient<TicketReplayProvider>();
        services.AddTransient<ITicketReplayProvider>(sp => sp.GetRequiredService<TicketReplayProvider>());
    }

    private static void AddFileSystemServices(this IServiceCollection services)
    {
        services.AddSingleton<ISerializationService, SerializationService>();
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
        services.AddSingleton<MainWindow>();
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
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<SettingsCompositeViewModel>();

        services.AddSingleton<WorkDaysViewModel>();
        services.AddSingleton<TimeTrackingViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<ExportViewModel>();

        services.AddSingleton<SettingsModel>();

        services.AddSingleton<DocumentationViewModel>();
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<DocumentationViewModel>());

        services.AddSingleton<DocumentationModel>();
        services.AddSingleton<IMessengerRegistration>(sp => sp.GetRequiredService<DocumentationModel>());
        services.AddSingleton<IInitializeAsync>(sp => sp.GetRequiredService<DocumentationModel>());

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
}