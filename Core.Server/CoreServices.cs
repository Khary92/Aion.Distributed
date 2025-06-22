using Core.Server.Communication.Services.AiSettings;
using Core.Server.Communication.Services.Note;
using Core.Server.Communication.Services.NoteType;
using Core.Server.Communication.Services.Settings;
using Core.Server.Communication.Services.Sprint;
using Core.Server.Communication.Services.Sprint.Handlers;
using Core.Server.Communication.Services.StatisticsData;
using Core.Server.Communication.Services.Tag;
using Core.Server.Communication.Services.Ticket;
using Core.Server.Communication.Services.TimerSettings;
using Core.Server.Communication.Services.TimeSlot;
using Core.Server.Communication.Services.TraceReport;
using Core.Server.Communication.Services.UseCase;
using Core.Server.Communication.Services.UseCase.Handler;
using Core.Server.Communication.Services.WorkDay;
using Core.Server.Services.Entities.AiSettings;
using Core.Server.Services.Entities.Notes;
using Core.Server.Services.Entities.NoteTypes;
using Core.Server.Services.Entities.Settings;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Services.Entities.Tags;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Services.Entities.TimerSettings;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Services.Entities.WorkDays;
using Core.Server.Services.UseCase;
using Core.Server.Translators.AiSettings;
using Core.Server.Translators.Notes;
using Core.Server.Translators.NoteTypes;
using Core.Server.Translators.Settings;
using Core.Server.Translators.Sprints;
using Core.Server.Translators.StatisticsData;
using Core.Server.Translators.Tags;
using Core.Server.Translators.Tickets;
using Core.Server.Translators.TimerSettings;
using Core.Server.Translators.TimeSlots;
using Core.Server.Translators.WorkDays;

namespace Core.Server;

public static class CoreServices
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        AddNotificationServices(services);
        AddCommonServices(services);
        AddRequestsServices(services);
        AddCommandsServices(services);
        AddCommandToEventTranslators(services);
        AddHandlers(services);
    }

    private static void AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<TimerService>();
        
        services.AddSingleton<IRunTimeSettings, RunTimeSettings>();
        services.AddScoped<ITimeSlotControlService, TimeSlotControlService>();
    }
    
    private static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<AddTicketToActiveSprintCommandHandler>();
        services.AddScoped<AddTicketToSprintCommandHandler>();
        services.AddScoped<SetSprintActiveStatusCommandHandler>();
        
        services.AddScoped<LoadTimeSlotControlDataHandler>();
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
        services.AddSingleton<NoteTypeNotificationService>();
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
}