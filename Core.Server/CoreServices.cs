using Microsoft.AspNetCore.Server.Kestrel.Core;
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
using Service.Server.Communication.Services.AiSettings;
using Service.Server.Communication.Services.Note;
using Service.Server.Communication.Services.NoteType;
using Service.Server.Communication.Services.Settings;
using Service.Server.Communication.Services.Sprint;
using Service.Server.Communication.Services.StatisticsData;
using Service.Server.Communication.Services.Tag;
using Service.Server.Communication.Services.Ticket;
using Service.Server.Communication.Services.TimerSettings;
using Service.Server.Communication.Services.TimeSlot;
using Service.Server.Communication.Services.TraceReport;
using Service.Server.Communication.Services.UseCase;
using Service.Server.Communication.Services.WorkDay;
using Service.Server.Services.Entities.AiSettings;
using Service.Server.Services.Entities.Notes;
using Service.Server.Services.Entities.NoteTypes;
using Service.Server.Services.Entities.Settings;
using Service.Server.Services.Entities.Sprints;
using Service.Server.Services.Entities.StatisticsData;
using Service.Server.Services.Entities.Tags;
using Service.Server.Services.Entities.Tickets;
using Service.Server.Services.Entities.TimerSettings;
using Service.Server.Services.Entities.TimeSlots;
using Service.Server.Services.Entities.WorkDays;
using Service.Server.Services.UseCase;
using Service.Server.Translators.AiSettings;
using Service.Server.Translators.Notes;
using Service.Server.Translators.NoteTypes;
using Service.Server.Translators.Settings;
using Service.Server.Translators.Sprints;
using Service.Server.Translators.StatisticsData;
using Service.Server.Translators.Tags;
using Service.Server.Translators.Tickets;
using Service.Server.Translators.TimerSettings;
using Service.Server.Translators.TimeSlots;
using Service.Server.Translators.WorkDays;

namespace Service.Server;

public static class CoreServices
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        AddNotificationServices(services);
        AddCommonServices(services);
        AddRequestsServices(services);
        AddCommandsServices(services);
        AddCommandToEventTranslators(services);
    }

    private static void AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IRunTimeSettings, RunTimeSettings>();
        services.AddSingleton<TimerService>();
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