using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Commands.AiSettings;
using Client.Avalonia.Communication.Commands.Notes;
using Client.Avalonia.Communication.Commands.NoteTypes;
using Client.Avalonia.Communication.Commands.Settings;
using Client.Avalonia.Communication.Commands.Sprints;
using Client.Avalonia.Communication.Commands.StatisticsData;
using Client.Avalonia.Communication.Commands.Tags;
using Client.Avalonia.Communication.Commands.Tickets;
using Client.Avalonia.Communication.Commands.TimerSettings;
using Client.Avalonia.Communication.Commands.TimeSlots;
using Client.Avalonia.Communication.Commands.TraceReports;
using Client.Avalonia.Communication.Commands.UseCases;
using Client.Avalonia.Communication.Commands.WorkDays;
using Client.Avalonia.Communication.Notifications.Notes;
using Client.Avalonia.Communication.Notifications.NoteType;
using Client.Avalonia.Communication.Notifications.Sprints;
using Client.Avalonia.Communication.Notifications.Tags;
using Client.Avalonia.Communication.Notifications.Ticket;
using Client.Avalonia.Communication.Notifications.TimerSettings;
using Client.Avalonia.Communication.Notifications.UseCase;
using Client.Avalonia.Communication.Notifications.WorkDay;
using Client.Avalonia.Communication.Requests.AiSettings;
using Client.Avalonia.Communication.Requests.Notes;
using Client.Avalonia.Communication.Requests.NoteTypes;
using Client.Avalonia.Communication.Requests.Settings;
using Client.Avalonia.Communication.Requests.Sprints;
using Client.Avalonia.Communication.Requests.StatisticsData;
using Client.Avalonia.Communication.Requests.Tags;
using Client.Avalonia.Communication.Requests.Tickets;
using Client.Avalonia.Communication.Requests.TimerSettings;
using Client.Avalonia.Communication.Requests.TimeSlots;
using Client.Avalonia.Communication.Requests.WorkDays;
using Client.Avalonia.Factories;
using Client.Avalonia.FileSystem;
using Client.Avalonia.FileSystem.Serializer;
using Client.Avalonia.ViewModels.Analysis;
using Client.Avalonia.ViewModels.Data;
using Client.Avalonia.ViewModels.Documentation;
using Client.Avalonia.ViewModels.Export;
using Client.Avalonia.ViewModels.Main;
using Client.Avalonia.ViewModels.Settings;
using Client.Avalonia.ViewModels.Synchronization;
using Client.Avalonia.ViewModels.TimeTracking;
using Client.Avalonia.ViewModels.TimeTracking.DynamicControls;
using Client.Avalonia.ViewModels.Tracing;
using Client.Avalonia.Views.Analysis;
using Client.Avalonia.Views.Data;
using Client.Avalonia.Views.Documentation;
using Client.Avalonia.Views.Export;
using Client.Avalonia.Views.Main;
using Client.Avalonia.Views.Setting;
using Client.Avalonia.Views.Tracking;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Microsoft.Extensions.DependencyInjection;
using Proto.Requests.NoteTypes;
using Proto.Requests.Settings;
using Proto.Requests.Sprints;
using Proto.Requests.StatisticsData;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using Proto.Requests.TimerSettings;
using Proto.Requests.TimeSlots;
using Proto.Requests.WorkDays;

namespace Client.Avalonia;

public static class Bootstrapper
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        AddSynchronizationServices(services);
        AddViews(services);
        AddModels(services);
        AddNotificationReceivers(services);
        AddRequestSenders(services);
        AddCommandSenders(services);
        AddFileSystemServices(services);
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
    }

    private static void AddViews(this IServiceCollection services)
    {
        services.AddSingleton<ContentWrapper>();
        services.AddSingleton<SettingsCompositeControl>();
        services.AddSingleton<TimeTrackingControl>();
        services.AddSingleton<DocumentationControl>();
        services.AddSingleton<DataCompositeControl>();
        services.AddSingleton<ExportControl>();
        services.AddSingleton<AnalysisControlWrapper>();
    }

    private static void AddModels(this IServiceCollection services)
    {
        services.AddSingleton<TracingViewModel>();
        services.AddSingleton<TracingModel>();

        services.AddSingleton<TimerSettingsViewModel>();
        services.AddSingleton<TimerSettingsModel>();

        services.AddSingleton<DocumentationViewModel>();
        services.AddSingleton<DocumentationModel>();

        services.AddTransient<TypeCheckBoxViewModel>();

        services.AddTransient<TimeSlotViewModel>();
        services.AddTransient<TimeSlotModel>();

        services.AddTransient<INoteStreamViewModelFactory, NoteStreamViewModelFactory>();
        services.AddTransient<NoteStreamViewModel>();

        services.AddTransient<INoteViewFactory, NoteViewFactory>();
        services.AddTransient<NoteViewModel>();

        services.AddTransient<ITagCheckBoxViewFactory, TagCheckBoxViewFactory>();
        services.AddTransient<TagCheckBoxViewModel>();

        services.AddSingleton<IStatisticsViewModelFactory, StatisticsViewModelFactory>();
        services.AddTransient<StatisticsViewModel>();

        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<SettingsModel>();

        services.AddSingleton<WorkDaysViewModel>();
        services.AddSingleton<WorkDaysModel>();

        services.AddSingleton<ITimeSlotViewModelFactory, TimeSlotViewModelFactory>();
        services.AddSingleton<TimeTrackingViewModel>();
        services.AddSingleton<TimeTrackingModel>();

        services.AddSingleton<ExportViewModel>();
        services.AddSingleton<ExportModel>();

        services.AddSingleton<AiSettingsViewModel>();
        services.AddSingleton<AiSettingsModel>();

        services.AddSingleton<SprintsDataViewModel>();
        services.AddSingleton<SprintsDataModel>();

        services.AddSingleton<TicketsDataViewModel>();
        services.AddSingleton<TicketsDataModel>();

        services.AddSingleton<TagsDataViewModel>();
        services.AddSingleton<TagsDataModel>();

        services.AddSingleton<NotesDataViewModel>();
        services.AddSingleton<NotesDataModel>();

        services.AddSingleton<AnalysisByTagViewModel>();
        services.AddSingleton<AnalysisByTagModel>();

        services.AddSingleton<AnalysisByTicketViewModel>();
        services.AddSingleton<AnalysisByTicketModel>();

        services.AddSingleton<AnalysisBySprintViewModel>();
        services.AddSingleton<AnalysisBySprintModel>();

        services.AddSingleton<AnalysisControlWrapperViewModel>();

        services.AddSingleton<ContentWrapperViewModel>();
        services.AddSingleton<DataCompositeViewModel>();
        services.AddSingleton<SettingsCompositeViewModel>();
    }

    private static void AddNotificationReceivers(this IServiceCollection services)
    {
        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        services.AddHostedService<NoteNotificationBackgroundService>();
        services.AddSingleton<NoteNotificationReceiver>();

        services.AddHostedService<NoteTypeNotificationBackgroundService>();
        services.AddSingleton<NoteTypeNotificationReceiver>();

        services.AddHostedService<SprintNotificationBackgroundService>();
        services.AddSingleton<SprintNotificationReceiver>();

        services.AddHostedService<TagNotificationBackgroundService>();
        services.AddSingleton<TagNotificationReceiver>();

        services.AddHostedService<TicketNotificationBackgroundService>();
        services.AddSingleton<TicketNotificationReceiver>();

        services.AddHostedService<TimerSettingsNotificationBackgroundService>();
        services.AddSingleton<TimerSettingsNotificationReceiver>();

        services.AddHostedService<UseCaseNotificationBackgroundService>();
        services.AddSingleton<UseCaseNotificationReceiver>();

        services.AddHostedService<WorkDayNotificationBackgroundService>();
        services.AddSingleton<WorkDayNotificationReceiver>();
    }
    
    private static void AddCommandSenders(this IServiceCollection services)
    {
        services.AddScoped<ICommandSender, CommandSender>();

        services.AddScoped<IAiSettingsCommandSender, AiSettingsCommandSender>();
        services.AddScoped<INoteCommandSender, NoteCommandSender>();
        services.AddScoped<INoteTypeCommandSender, NoteTypeCommandSender>();
        services.AddScoped<ISettingsCommandSender, SettingsCommandSender>();
        services.AddScoped<ISprintCommandSender, SprintCommandSender>();
        services.AddScoped<IStatisticsDataCommandSender, StatisticsDataCommandSender>();
        services.AddScoped<ITagCommandSender, TagCommandSender>();
        services.AddScoped<ITicketCommandSender, TicketCommandSender>();
        services.AddScoped<ITimerSettingsCommandSender, TimerSettingsCommandSender>();
        services.AddScoped<ITimeSlotCommandSender, TimeSlotCommandSender>();
        services.AddScoped<ITraceReportCommandSender, TraceReportCommandSender>();
        services.AddScoped<IUseCaseCommandSender, UseCaseCommandSender>();
        services.AddScoped<IWorkDayCommandSender, WorkDayCommandSender>();
    }
    
    private static void AddRequestSenders(this IServiceCollection services)
    {
        services.AddScoped<IAiSettingsRequestSender, AiSettingsRequestSender>();
        services.AddScoped<INotesRequestSender, NotesRequestSender>();
        services.AddScoped<INoteTypesRequestSender, NoteTypesRequestSender>();
        services.AddScoped<ISettingsRequestSender, SettingsRequestSender>();
        services.AddScoped<ISprintRequestSender, SprintRequestSender>();
        services.AddScoped<IStatisticsDataRequestSender, StatisticsDataRequestSender>();
        services.AddScoped<ITagRequestSender, TagRequestSender>();
        services.AddScoped<ITicketRequestSender, TicketRequestSender>();
        services.AddScoped<ITimerSettingsRequestSender, TimerSettingsRequestSender>();
        services.AddScoped<ITimeSlotRequestSender, TimeSlotRequestSender>();
        services.AddScoped<IWorkDayRequestSender, WorkDayRequestSender>();
    }
}