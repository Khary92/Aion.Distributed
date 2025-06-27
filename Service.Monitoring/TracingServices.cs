using Microsoft.Extensions.DependencyInjection;
using Service.Monitoring.Tracers;
using Service.Monitoring.Tracers.AiSettings;
using Service.Monitoring.Tracers.Export;
using Service.Monitoring.Tracers.Note;
using Service.Monitoring.Tracers.NoteType;
using Service.Monitoring.Tracers.Sprint;
using Service.Monitoring.Tracers.Tag;
using Service.Monitoring.Tracers.Ticket;
using Service.Monitoring.Tracers.TimerSettings;
using Service.Monitoring.Tracers.WorkDay;

namespace Service.Monitoring;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddASinks(services);
    }

    private static void AddASinks(this IServiceCollection services)
    {
        services.AddSingleton<ITraceSink, AiSettingsTraceSink>();
        services.AddSingleton<ITraceSink, ExportTraceSink>();
        services.AddSingleton<ITraceSink, NoteTraceSink>();
        services.AddSingleton<ITraceSink, SprintTraceSink>();
        services.AddSingleton<ITraceSink, NoteTypeTraceSink>();
        services.AddSingleton<ITraceSink, TagTraceSink>();
        services.AddSingleton<ITraceSink, TicketTraceSink>();
        services.AddSingleton<ITraceSink, TimerSettingsTraceSink>();
        services.AddSingleton<ITraceSink, WorkDayTraceSink>();
    }
}