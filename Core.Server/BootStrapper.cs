using Microsoft.AspNetCore.Server.Kestrel.Core;
using Proto.Requests.AnalysisData;
using Proto.Requests.Tags;
using Proto.Requests.TimerSettings;
using Proto.Requests.TimeSlots;
using Proto.Requests.UseCase;
using Proto.Requests.WorkDays;
using Service.Server.Communication.AiSettings;
using Service.Server.Communication.Mock.AiSettings;
using Service.Server.Communication.Mock.Analysis;
using Service.Server.Communication.Mock.Note;
using Service.Server.Communication.Mock.NoteType;
using Service.Server.Communication.Mock.Settings;
using Service.Server.Communication.Mock.Sprint;
using Service.Server.Communication.Mock.StatisticsData;
using Service.Server.Communication.Mock.Tag;
using Service.Server.Communication.Mock.Ticket;
using Service.Server.Communication.Mock.TimerSettings;
using Service.Server.Communication.Mock.TimeSlot;
using Service.Server.Communication.Mock.TraceReport;
using Service.Server.Communication.Mock.UseCase;
using Service.Server.Communication.Mock.WorkDay;
using Service.Server.Communication.Note;
using Service.Server.Communication.NoteType;
using Service.Server.Communication.Settings;
using Service.Server.Communication.Sprint;
using Service.Server.Communication.StatisticsData;
using Service.Server.Communication.Tag;
using Service.Server.Communication.Ticket;
using Service.Server.Communication.TimerSettings;
using Service.Server.Communication.TimeSlot;
using Service.Server.Communication.TraceReport;
using Service.Server.Communication.UseCase;
using Service.Server.Communication.WorkDay;
using Service.Server.Old.Services.Entities.AiSettings;
using Service.Server.Old.Services.Entities.Notes;
using Service.Server.Old.Services.Entities.NoteTypes;
using Service.Server.Old.Services.Entities.Settings;
using Service.Server.Old.Services.Entities.Sprints;
using Service.Server.Old.Services.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.Tags;
using Service.Server.Old.Services.Entities.Tickets;
using Service.Server.Old.Services.Entities.TimerSettings;
using Service.Server.Old.Services.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.WorkDays;
using Service.Server.Old.Services.UseCase;
using Service.Server.Old.Services.UseCase.Replays;
using Service.Server.Old.Translators.AiSettings;
using Service.Server.Old.Translators.Notes;
using Service.Server.Old.Translators.NoteTypes;
using Service.Server.Old.Translators.Settings;
using Service.Server.Old.Translators.Sprints;
using Service.Server.Old.Translators.StatisticsData;
using Service.Server.Old.Translators.Tags;
using Service.Server.Old.Translators.Tickets;
using Service.Server.Old.Translators.TimerSettings;
using Service.Server.Old.Translators.TimeSlots;
using Service.Server.Old.Translators.WorkDays;

namespace Service.Server;

public static class BootStrapper
{
    public static WebApplication BuildWebApp(String[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options => { options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2); });
        AddNotificationServices(builder.Services);
        AddCommonServices(builder.Services);
        AddRequestsServices(builder.Services);
        AddCommandsServices(builder.Services);
        AddCommandToEventTranslators(builder.Services);
        return builder.Build();
    }
    
    private static void AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IRunTimeSettings, RunTimeSettings>();
        services.AddSingleton<IReplayRequestsService, ReplayRequestsService>();
    }

    private static void AddRequestsServices(this IServiceCollection services)
    {
        services.AddSingleton<IAiSettingsRequestsService, AiSettingsRequestsService>();
        services.AddSingleton<INoteRequestsService, NoteRequestsService>();
        services.AddSingleton<INoteTypeRequestsService, NoteTypeRequestsService>();
        services.AddSingleton<ISprintRequestsService, SprintRequestService>();
        services.AddSingleton<ITicketRequestsService, TicketRequestService>();
        services.AddSingleton<IWorkDayRequestsService, WorkDayRequestsService>();
        services.AddSingleton<ITagRequestsService, TagRequestsService>();
        services.AddSingleton<ITimeSlotRequestsService, TimeSlotRequestsService>();
        services.AddSingleton<IStatisticsDataRequestsService, StatisticsDataRequestsService>();
        services.AddSingleton<ISettingsRequestsService, SettingsRequestsService>();
        services.AddSingleton<ITimerSettingsRequestsService, TimerSettingsRequestsService>();
    }

    private static void AddCommandsServices(this IServiceCollection services)
    {
        services.AddSingleton<IAiSettingsCommandsService, AiSettingsCommandsService>();
        services.AddSingleton<INoteCommandsService, NoteCommandsService>();
        services.AddSingleton<INoteTypeCommandsService, NoteTypeCommandsService>();
        services.AddSingleton<ISprintCommandsService, SprintCommandsService>();
        services.AddSingleton<ITicketCommandsService, TicketCommandsService>();
        services.AddSingleton<IWorkDayCommandsService, WorkDayCommandsService>();
        services.AddSingleton<ITagCommandsService, TagCommandsService>();
        services.AddSingleton<ITimeSlotCommandsService, TimeSlotCommandService>();
        services.AddSingleton<IStatisticsDataCommandsService, StatisticsDataCommandsService>();
        services.AddSingleton<ISettingsCommandsService, SettingsCommandsService>();
        services.AddSingleton<ITimerSettingsCommandsService, TimerSettingsCommandsService>();
    }
    
    private static void AddCommandToEventTranslators(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsCommandsToEventTranslator, SettingsCommandsToEventTranslator>();
        services.AddSingleton<INoteCommandsToEventTranslator, NoteCommandsToEventTranslator>();
        services.AddSingleton<ISprintCommandsToEventTranslator, SprintCommandsToEventTranslator>();
        services.AddSingleton<IStatisticsDataCommandsToEventTranslator, StatisticsDataCommandsToEventTranslator>();
        services.AddSingleton<ITagCommandsToEventTranslator, TagCommandsToEventTranslator>();
        services.AddSingleton<ITicketCommandsToEventTranslator, TicketCommandsToEventTranslator>();
        services.AddSingleton<ITimeSlotCommandsToEventTranslator, TimeSlotCommandsToEventTranslator>();
        services.AddSingleton<IWorkDayCommandsToEventTranslator, WorkDayCommandsToEventTranslator>();
        services.AddSingleton<IAiSettingsCommandsToEventTranslator, AiSettingsCommandsToEventTranslator>();
        services.AddSingleton<INoteTypeCommandsToEventTranslator, NoteTypeCommandsToEventTranslator>();
        services.AddSingleton<ITimerSettingsCommandsToEventTranslator, TimerSettingsCommandsToEventTranslator>();
    }
    
    private static void AddNotificationServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        
        services.AddSingleton<AiSettingsNotificationService>();
        services.AddSingleton<NoteNotificationService>();
        services.AddSingleton<NoteTypeNotificationServiceImpl>();
        services.AddSingleton<SettingsNotificationService>();
        services.AddSingleton<SprintNotificationService>();
        services.AddSingleton<StatisticsDataNotificationService>();
        services.AddSingleton<TagNotificationServiceImpl>();
        services.AddSingleton<TicketNotificationService>();
        services.AddSingleton<TimerSettingsNotificationService>();
        services.AddSingleton<TimeSlotNotificationService>();
        services.AddSingleton<TraceReportNotificationService>();
        services.AddSingleton<UseCaseNotificationService>();
        services.AddSingleton<WorkDayNotificationService>();
    }

    public static void AddEndPoints(this WebApplication app)
    {
        AddCommandEndPoints(app);
    }
    
    private static void AddCommandEndPoints(WebApplication app)
    {
        app.MapGrpcService<AiSettingsCommandReceiver>();
        app.MapGrpcService<NoteCommandReceiver>();
        app.MapGrpcService<NoteTypeCommandReceiver>();
        app.MapGrpcService<SettingsCommandReceiver>();
        app.MapGrpcService<SprintCommandReceiver>();
        app.MapGrpcService<StatisticsDataCommandReceiver>();
        app.MapGrpcService<TagCommandReceiver>();
        app.MapGrpcService<TicketCommandReceiver>();
        app.MapGrpcService<TimerSettingsCommandReceiver>();
        app.MapGrpcService<TimeSlotCommandService>();
        app.MapGrpcService<UseCaseCommandReceiver>();
        app.MapGrpcService<WorkDayCommandReceiver>();
        
    }
    
    private static void AddRequestEndPoints(WebApplication app)
    {
        app.MapGrpcService<AiSettingsRequestReceiver>();
        app.MapGrpcService<NoteRequestReceiver>();
        app.MapGrpcService<NoteTypeRequestReceiver>();
        app.MapGrpcService<SettingsRequestReceiver>();
        app.MapGrpcService<SprintRequestService>();
        app.MapGrpcService<StatisticsDataRequestReceiver>();
        app.MapGrpcService<TagRequestReceiver>();
        app.MapGrpcService<TicketRequestReceiver>();
        app.MapGrpcService<TimerSettingsRequestReceiver>();
        app.MapGrpcService<TimeSlotRequestReceiver>();
        app.MapGrpcService<WorkDayRequestReceiver>();
        app.MapGrpcService<UseCaseRequestReceiver>();
        app.MapGrpcService<AnalysisRequestReceiver>();
    }

    public static void AddNotificationEndPoints(WebApplication app)
    {
        app.MapGrpcService<AiSettingsNotificationService>();
        app.MapGrpcService<NoteNotificationService>();
        app.MapGrpcService<NoteTypeNotificationServiceImpl>();
        app.MapGrpcService<SettingsNotificationService>();
        app.MapGrpcService<SprintNotificationService>();
        app.MapGrpcService<StatisticsDataNotificationService>();
        app.MapGrpcService<TagNotificationServiceImpl>();
        app.MapGrpcService<TicketNotificationService>();
        app.MapGrpcService<TimerSettingsNotificationService>();
        app.MapGrpcService<TimeSlotNotificationService>();
        app.MapGrpcService<TraceReportNotificationService>();
        app.MapGrpcService<UseCaseNotificationService>();
        app.MapGrpcService<WorkDayNotificationService>();
    }

    public static void AddMockingEndpoints(this WebApplication app)
    {
        app.MapGrpcService<MockAiSettingsCommandReceiver>();
        app.MapGrpcService<MockNoteCommandReceiver>();
        app.MapGrpcService<MockNoteTypeCommandService>();
        app.MapGrpcService<MockSettingsCommandService>();
        app.MapGrpcService<MockSprintCommandService>();
        app.MapGrpcService<MockStatisticsDataCommandService>();
        app.MapGrpcService<MockTagCommandService>();
        app.MapGrpcService<MockTicketCommandService>();
        app.MapGrpcService<MockTimerSettingsCommandService>();
        app.MapGrpcService<MockTimeSlotCommandService>();
        app.MapGrpcService<MockTraceReportCommandService>();
        app.MapGrpcService<MockUseCaseCommandService>();
        app.MapGrpcService<MockWorkDayCommandService>();

        app.MapGrpcService<MockAiSettingsRequestReceiver>();
        app.MapGrpcService<NoteRequestReceiver>();
        app.MapGrpcService<MockNoteTypeRequestService>();
        app.MapGrpcService<MockSettingsRequestService>();
        app.MapGrpcService<MockSprintRequestService>();
        app.MapGrpcService<MockStatisticsDataRequestService>();
        app.MapGrpcService<MockTagRequestService>();
        app.MapGrpcService<MockTicketRequestService>();
        app.MapGrpcService<MockTimerSettingsRequestService>();
        app.MapGrpcService<MockTimeSlotRequestService>();
        app.MapGrpcService<MockWorkDayRequestService>();
        app.MapGrpcService<MockUseCaseRequestService>();
        app.MapGrpcService<MockAnalysisRequestService>();
    }
}