using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.AiSettings;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.NoteTypes;
using Client.Desktop.Communication.Commands.Settings;
using Client.Desktop.Communication.Commands.Sprints;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.Tags;
using Client.Desktop.Communication.Commands.Tickets;
using Client.Desktop.Communication.Commands.TimerSettings;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TraceReports;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.WorkDays;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.AiSettings;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.NoteTypes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.Settings;
using Client.Desktop.Communication.Requests.Sprints;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.Tags;
using Client.Desktop.Communication.Requests.Tickets;
using Client.Desktop.Communication.Requests.TimerSettings;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.UseCase;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.Communication.RequiresChange;
using Client.Desktop.Factories;
using Client.Desktop.FileSystem;
using Client.Desktop.FileSystem.Serializer;
using Client.Desktop.Models.Analysis;
using Client.Desktop.Models.Data;
using Client.Desktop.Models.Documentation;
using Client.Desktop.Models.Export;
using Client.Desktop.Models.Main;
using Client.Desktop.Models.Settings;
using Client.Desktop.Models.Synchronization;
using Client.Desktop.Models.TimeTracking;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using Client.Desktop.Replays;
using Client.Desktop.Services;
using Client.Desktop.Services.Cache;
using Client.Desktop.Views.Analysis;
using Client.Desktop.Views.Data;
using Client.Desktop.Views.Documentation;
using Client.Desktop.Views.Export;
using Client.Desktop.Views.Main;
using Client.Desktop.Views.Setting;
using Client.Desktop.Views.Tracking;
using CommunityToolkit.Mvvm.Messaging;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Proto.Command.TimeSlots;
using Proto.Notifications.AiSettings;
using Proto.Notifications.Note;
using Proto.Notifications.NoteType;
using Proto.Notifications.Settings;
using Proto.Notifications.Sprint;
using Proto.Notifications.Tag;
using Proto.Notifications.Ticket;
using Proto.Notifications.TimerSettings;
using Proto.Notifications.UseCase;
using Proto.Notifications.WorkDay;
using Proto.Shared;

namespace Client.Desktop;

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

        //TODO fix it
        services.AddSingleton<IRunTimeSettings, RunTimeSettings>();
        services.AddSingleton<IExportService, ExportService>();
        services.AddSingleton<ILanguageModelApi, LanguageModelApiStub>();
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
        services.AddSingleton<IPersistentCache<SetStartTimeCommandProto>, StartTimeChangedCache>();
        services.AddSingleton<IPersistentCache<SetEndTimeCommandProto>, EndTimeChangedCache>();
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

        services.AddSingleton<NotificationReceiverStarter>();

        services.AddSingleton<AiSettingsNotificationReceiver>();
        services.AddSingleton<SettingsNotificationReceiver>();
        services.AddSingleton<TicketNotificationReceiver>();
        services.AddSingleton<NoteNotificationReceiver>();
        services.AddSingleton<NoteTypeNotificationReceiver>();
        services.AddSingleton<SprintNotificationReceiver>();
        services.AddSingleton<TagNotificationReceiver>();
        services.AddSingleton<TimerSettingsNotificationReceiver>();
        services.AddSingleton<UseCaseNotificationReceiver>();
        services.AddSingleton<WorkDayNotificationReceiver>();

        var channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
        services.AddSingleton(new AiSettingsNotificationService.AiSettingsNotificationServiceClient(channel));
        services.AddSingleton(new SettingsNotificationService.SettingsNotificationServiceClient(channel));
        services.AddSingleton(new TicketNotificationService.TicketNotificationServiceClient(channel));
        services.AddSingleton(new NoteNotificationService.NoteNotificationServiceClient(channel));
        services.AddSingleton(new NoteTypeNotificationService.NoteTypeNotificationServiceClient(channel));
        services.AddSingleton(new SprintNotificationService.SprintNotificationServiceClient(channel));
        services.AddSingleton(new TagNotificationService.TagNotificationServiceClient(channel));
        services.AddSingleton(new TimerSettingsNotificationService.TimerSettingsNotificationServiceClient(channel));
        services.AddSingleton(new UseCaseNotificationService.UseCaseNotificationServiceClient(channel));
        services.AddSingleton(new WorkDayNotificationService.WorkDayNotificationServiceClient(channel));
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
        services.AddScoped<IRequestSender, RequestSender>();

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
        services.AddScoped<ITicketReplayRequestSender, TicketReplayRequestSender>();
        services.AddScoped<ITicketReplayRequestSender, TicketReplayRequestSender>();
        services.AddScoped<IUseCaseRequestSender, UseCaseRequestSender>();
        services.AddScoped<IAnalysisRequestSender, AnalysisRequestSender>();
    }
}