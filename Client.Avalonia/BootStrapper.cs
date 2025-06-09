using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Notifications.Notes;
using Client.Avalonia.Communication.Sender;
using Client.Avalonia.Factories;
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

namespace Client.Avalonia;

public static class Bootstrapper
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        AddSynchronizationServices(services);
        AddViews(services);
        AddModels(services);
        AddNotificationReceivers(services);
        AddCommandSenders(services);
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
}