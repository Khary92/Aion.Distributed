using Client.Tracing.Tracing.Tracers;
using Client.Tracing.Tracing.Tracers.Export;
using Client.Tracing.Tracing.Tracers.Export.UseCase;
using Client.Tracing.Tracing.Tracers.Note;
using Client.Tracing.Tracing.Tracers.Note.UseCase;
using Client.Tracing.Tracing.Tracers.NoteType;
using Client.Tracing.Tracing.Tracers.NoteType.UseCase;
using Client.Tracing.Tracing.Tracers.Sprint;
using Client.Tracing.Tracing.Tracers.Sprint.UseCase;
using Client.Tracing.Tracing.Tracers.Tag;
using Client.Tracing.Tracing.Tracers.Tag.UseCase;
using Client.Tracing.Tracing.Tracers.Ticket;
using Client.Tracing.Tracing.Tracers.Ticket.UseCase;
using Client.Tracing.Tracing.Tracers.TimerSettings;
using Client.Tracing.Tracing.Tracers.TimerSettings.UseCase;
using Client.Tracing.Tracing.Tracers.WorkDay;
using Client.Tracing.Tracing.Tracers.WorkDay.UseCase;
using Microsoft.Extensions.DependencyInjection;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddACommonTracingServices(services);
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
        services.AddScoped<ITracingDataCommandSender>(sp => new TracingDataCommandSender("http://127.0.0.1:8082"));
        services.AddScoped<ITraceCollector, TraceCollector>();
    }
    
    private static void AddExportTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<IExportTraceCollector, ExportTraceCollector>();

        services.AddSingleton<IExportUseCaseSelector, ExportUseCaseSelector>();
    }

    private static void AddNoteTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateNoteTraceCollector, CreateNoteTraceCollector>();
        services.AddSingleton<IUpdateNoteTraceCollector, UpdateNoteTraceCollector>();

        services.AddSingleton<INoteUseCaseSelector, NoteUseCaseSelector>();
    }

    private static void AddNoteTypeTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateNoteTypeTraceCollector, CreateNoteTypeTraceCollector>();
        services.AddSingleton<IChangeNoteTypeColorTraceCollector, ChangeNoteTypeColorTraceCollector>();
        services.AddSingleton<IChangeNoteTypeNameTraceCollector, ChangeNoteTypeNameTraceCollector>();

        services.AddSingleton<INoteTypeUseCaseSelector, NoteTypeUseCaseSelector>();
    }

    private static void AddSprintTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateSprintTraceCollector, CreateSprintTraceCollector>();
        services.AddSingleton<ISprintActiveStatusCollector, SprintActiveStatusCollector>();
        services.AddSingleton<ITicketAddedToSprintCollector, TicketAddedToSprintCollector>();
        services.AddSingleton<IUpdateSprintCollector, UpdateSprintCollector>();

        services.AddSingleton<ISprintUseCaseSelector, SprintUseCaseSelector>();
    }

    private static void AddTagTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateTagTraceCollector, CreateTagTraceCollector>();
        services.AddSingleton<IUpdateTagTraceCollector, UpdateTagTraceCollector>();

        services.AddSingleton<ITagUseCaseSelector, TagUseCaseSelector>();
    }

    private static void AddTicketTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateTicketTraceCollector, CreateTicketTraceCollector>();
        services.AddSingleton<IAddTicketToCurrentSprintTraceCollector, AddTicketToCurrentSprintTraceCollector>();
        services.AddSingleton<IUpdateTicketTraceCollector, UpdateTicketTraceCollector>();

        services.AddSingleton<ITicketUseCaseSelector, TicketUseCaseSelector>();
    }

    private static void AddTimerSettingsTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateTimerSettingsTraceCollector, CreateTimerSettingsTraceCollector>();
        services.AddSingleton<IChangeDocuTimerSaveIntervalTraceCollector, ChangeDocuTimerSaveIntervalTraceCollector>();
        services.AddSingleton<IChangeSnapshotSaveIntervalTraceCollector, ChangeSnapshotSaveIntervalTraceCollector>();

        services.AddSingleton<ITimerSettingsUseCaseSelector, TimerSettingsUseCaseSelector>();
    }

    private static void AddWorkdayTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateWorkDayTraceCollector, CreateWorkDayTraceCollector>();
        services.AddSingleton<IWorkDayUseCaseSelector, WorkDayUseCaseSelector>();
    }
}