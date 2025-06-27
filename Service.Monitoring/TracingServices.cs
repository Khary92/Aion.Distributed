using Microsoft.Extensions.DependencyInjection;
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

namespace Service.Monitoring;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddASinks(services);
    }

    private static void AddASinks(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink<AiSettingsTraceRecord>, AiSettingsTraceSink>();
        services.AddSingleton<ITraceSink<ExportTraceRecord>, ExportTraceSink>();
        services.AddSingleton<ITraceSink<NoteTraceRecord>, NoteTraceSink>();
        services.AddSingleton<ITraceSink<NoteTypeTraceRecord>, NoteTypeTraceSink>();
        services.AddSingleton<ITraceSink<SprintTraceRecord>, SprintTraceSink>();
        services.AddSingleton<ITraceSink<TagTraceRecord>, TagTraceSink>();
        services.AddSingleton<ITraceSink<TicketTraceRecord>, TicketTraceSink>();
        services.AddSingleton<ITraceSink<TimerSettingsTraceRecord>, TimerSettingsTraceSink>();
        services.AddSingleton<ITraceSink<WorkDayTraceRecord>, WorkDayTraceSink>();
    }
}