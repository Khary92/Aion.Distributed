using Service.Monitoring.Tracers.AiSettings;
using Service.Monitoring.Tracers.Export;
using Service.Monitoring.Tracers.Note;
using Service.Monitoring.Tracers.NoteType;
using Service.Monitoring.Tracers.Sprint;
using Service.Monitoring.Tracers.Tag;
using Service.Monitoring.Tracers.Ticket;
using Service.Monitoring.Tracers.TimerSettings;
using Service.Monitoring.Tracers.WorkDay;
using NoteTraceRecord = Service.Monitoring.Tracers.Note.NoteTraceRecord;
using TicketUseCaseSelector = Tracing.Tracers.Ticket.TicketUseCaseSelector;

namespace Service.Monitoring;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddACommonTracingServices(services);
        AddAiSettingsTracingServices(services);
        AddExportTracingServices(services);
        AddNoteTracingServices(services);
        AddNoteTypeTracingServices(services);
        AddSprintTracingServices(services);
        AddTagTracingServices(services);
        AddTicketTracingServices(services);
        AddTimerSettingsTracingServices(services);
        AddWorkdayTracingServices(services);
    }

    private static void AddACommonTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITracingCollectorProvider, TracingCollectorProvider>();
    }

    private static void AddAiSettingsTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<AiSettingsTraceRecord>, AiSettingsTraceSink>();

        services.AddSingleton<IChangeLanguageModelTraceCollector, ChangeLanguageModelTraceCollector>();
        services.AddSingleton<IChangePromptTraceCollector, ChangePromptTraceCollector>();

        services.AddSingleton<IAiSettingsUseCaseSelector, AiSettingsUseCaseSelector>();
    }

    private static void AddExportTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<ExportTraceRecord>, ExportTraceSink>();

        services.AddSingleton<IExportTraceCollector, ExportTraceCollector>();

        services.AddSingleton<IExportUseCaseSelector, ExportUseCaseSelector>();
    }

    private static void AddNoteTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<NoteTraceRecord>, NoteTraceSink>();

        services.AddSingleton<ICreateNoteTraceCollector, CreateNoteTraceCollector>();
        services.AddSingleton<IUpdateNoteTraceCollector, UpdateNoteTraceCollector>();

        services.AddSingleton<INoteUseCaseSelector, NoteUseCaseSelector>();
    }

    private static void AddNoteTypeTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<NoteTypeTraceRecord>, NoteTypeTraceSink>();

        services.AddSingleton<ICreateNoteTypeTraceCollector, CreateNoteTypeTraceCollector>();
        services.AddSingleton<IChangeNoteTypeColorTraceCollector, ChangeNoteTypeColorTraceCollector>();
        services.AddSingleton<IChangeNoteTypeNameTraceCollector, ChangeNoteTypeNameTraceCollector>();

        services.AddSingleton<INoteTypeUseCaseSelector, NoteTypeUseCaseSelector>();
    }

    private static void AddSprintTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<SprintTraceRecord>, SprintTraceSink>();

        services.AddSingleton<ICreateSprintTraceCollector, CreateSprintTraceCollector>();
        services.AddSingleton<ISprintActiveStatusCollector, SprintActiveStatusCollector>();
        services.AddSingleton<ITicketAddedToSprintCollector, TicketAddedToSprintCollector>();
        services.AddSingleton<IUpdateSprintCollector, UpdateSprintCollector>();

        services.AddSingleton<ISprintUseCaseSelector, SprintUseCaseSelector>();
    }

    private static void AddTagTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<TagTraceRecord>, TagTraceSink>();

        services.AddSingleton<ICreateTagTraceCollector, CreateTagTraceCollector>();
        services.AddSingleton<IUpdateTagTraceCollector, UpdateTagTraceCollector>();

        services.AddSingleton<ITagUseCaseSelector, TagUseCaseSelector>();
    }

    private static void AddTicketTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<TicketTraceRecord>, TicketTraceSink>();

        services.AddSingleton<ICreateTicketTraceCollector, CreateTicketTraceCollector>();
        services.AddSingleton<IAddTicketToCurrentSprintTraceCollector, AddTicketToCurrentSprintTraceCollector>();
        services.AddSingleton<IUpdateTicketTraceCollector, UpdateTicketTraceCollector>();

        services.AddSingleton<ITicketUseCaseSelector, TicketUseCaseSelector>();
    }

    private static void AddTimerSettingsTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<TimerSettingsTraceRecord>, TimerSettingsTraceSink>();

        services.AddSingleton<ICreateTimerSettingsTraceCollector, CreateTimerSettingsTraceCollector>();
        services.AddSingleton<IChangeDocuTimerSaveIntervalTraceCollector, ChangeDocuTimerSaveIntervalTraceCollector>();
        services.AddSingleton<IChangeSnapshotSaveIntervalTraceCollector, ChangeSnapshotSaveIntervalTraceCollector>();

        services.AddSingleton<ITimerSettingsUseCaseSelector, TimerSettingsUseCaseSelector>();
    }

    private static void AddWorkdayTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<WorkDayTraceRecord>, WorkDayTraceSink>();

        services.AddSingleton<ICreateWorkDayTraceCollector, CreateWorkDayTraceCollector>();

        services.AddSingleton<IWorkDayUseCaseSelector, WorkDayUseCaseSelector>();
    }
}