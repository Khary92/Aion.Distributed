using Core.Server.Communication.Endpoints.AiSettings;
using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Endpoints.NoteType;
using Core.Server.Communication.Endpoints.Settings;
using Core.Server.Communication.Endpoints.Sprint;
using Core.Server.Communication.Endpoints.Sprint.Handlers;
using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TimerSettings;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Endpoints.TraceReport;
using Core.Server.Communication.Endpoints.UseCase;
using Core.Server.Communication.Endpoints.UseCase.Handler;
using Core.Server.Communication.Endpoints.WorkDay;
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
using Core.Server.Translators.Commands.AiSettings;
using Core.Server.Translators.Commands.Notes;
using Core.Server.Translators.Commands.NoteTypes;
using Core.Server.Translators.Commands.Settings;
using Core.Server.Translators.Commands.Sprints;
using Core.Server.Translators.Commands.StatisticsData;
using Core.Server.Translators.Commands.Tags;
using Core.Server.Translators.Commands.Tickets;
using Core.Server.Translators.Commands.TimerSettings;
using Core.Server.Translators.Commands.TimeSlots;
using Core.Server.Translators.Commands.WorkDays;

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