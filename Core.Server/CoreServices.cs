using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Endpoints.NoteType;
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
using Core.Server.Services.Entities.Notes;
using Core.Server.Services.Entities.NoteTypes;
using Core.Server.Services.Entities.Sprints;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Services.Entities.Tags;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Services.Entities.TimerSettings;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Services.Entities.WorkDays;
using Core.Server.Services.UseCase;
using Core.Server.Translators.Commands.Notes;
using Core.Server.Translators.Commands.NoteTypes;
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
        services.AddSingleton<IRunTimeSettings, RunTimeSettings>();

        services.AddScoped<TimerService>();
        services.AddScoped<ITimeSlotControlService, TimeSlotControlService>();
        services.AddScoped<IAnalysisDataService, AnalysisDataService>();
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
        services.AddScoped<INoteRequestsService, NoteRequestsService>();
        services.AddScoped<INoteTypeRequestsService, NoteTypeRequestsService>();
        services.AddScoped<ISprintRequestsService, SprintRequestService>();
        services.AddScoped<ITicketRequestsService, TicketRequestService>();
        services.AddScoped<IWorkDayRequestsService, WorkDayRequestsService>();
        services.AddScoped<ITagRequestsService, TagRequestsService>();
        services.AddScoped<ITimeSlotRequestsService, TimeSlotRequestsService>();
        services.AddScoped<IStatisticsDataRequestsService, StatisticsDataRequestsService>();
        services.AddScoped<ITimerSettingsRequestsService, TimerSettingsRequestsService>();
    }

    private static void AddCommandsServices(this IServiceCollection services)
    {
        services.AddScoped<INoteCommandsService, NoteCommandsService>();
        services.AddScoped<INoteTypeCommandsService, NoteTypeCommandsService>();
        services.AddScoped<ISprintCommandsService, SprintCommandsService>();
        services.AddScoped<ITicketCommandsService, TicketCommandsService>();
        services.AddScoped<IWorkDayCommandsService, WorkDayCommandsService>();
        services.AddScoped<ITagCommandsService, TagCommandsService>();
        services.AddScoped<ITimeSlotCommandsService, TimeSlotCommandService>();
        services.AddScoped<IStatisticsDataCommandsService, StatisticsDataCommandsService>();
        services.AddScoped<ITimerSettingsCommandsService, TimerSettingsCommandsService>();
    }

    private static void AddCommandToEventTranslators(this IServiceCollection services)
    {
        services.AddSingleton<INoteCommandsToEventTranslator, NoteCommandsToEventTranslator>();
        services.AddSingleton<ISprintCommandsToEventTranslator, SprintCommandsToEventTranslator>();
        services.AddSingleton<IStatisticsDataCommandsToEventTranslator, StatisticsDataCommandsToEventTranslator>();
        services.AddSingleton<ITagCommandsToEventTranslator, TagCommandsToEventTranslator>();
        services.AddSingleton<ITicketCommandsToEventTranslator, TicketCommandsToEventTranslator>();
        services.AddSingleton<ITimeSlotCommandsToEventTranslator, TimeSlotCommandsToEventTranslator>();
        services.AddSingleton<IWorkDayCommandsToEventTranslator, WorkDayCommandsToEventTranslator>();
        services.AddSingleton<INoteTypeCommandsToEventTranslator, NoteTypeCommandsToEventTranslator>();
        services.AddSingleton<ITimerSettingsCommandsToEventTranslator, TimerSettingsCommandsToEventTranslator>();
    }

    private static void AddNotificationServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();

        services.AddSingleton<NoteNotificationService>();
        services.AddSingleton<NoteTypeNotificationService>();
        services.AddSingleton<SprintNotificationService>();
        services.AddSingleton<StatisticsDataNotificationService>();
        services.AddSingleton<TagNotificationService>();
        services.AddSingleton<TicketNotificationService>();
        services.AddSingleton<TimerSettingsNotificationService>();
        services.AddSingleton<TimeSlotNotificationService>();
        services.AddSingleton<TraceReportNotificationService>();
        services.AddSingleton<UseCaseNotificationService>();
        services.AddSingleton<WorkDayNotificationService>();
    }
}