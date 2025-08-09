using Microsoft.Extensions.DependencyInjection;
using Service.Admin.Tracing.Tracing.NoteType;
using Service.Admin.Tracing.Tracing.NoteType.UseCase;
using Service.Admin.Tracing.Tracing.Sprint;
using Service.Admin.Tracing.Tracing.Sprint.UseCase;
using Service.Admin.Tracing.Tracing.Tag;
using Service.Admin.Tracing.Tracing.Tag.UseCase;
using Service.Admin.Tracing.Tracing.Ticket;
using Service.Admin.Tracing.Tracing.Ticket.UseCase;
using Service.Admin.Tracing.Tracing.TimerSettings;
using Service.Admin.Tracing.Tracing.TimerSettings.UseCase;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddACommonTracingServices(services);
        AddTicketTracingServices(services);
    }

    private static void AddACommonTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITracingDataCommandSender>(sp =>
            new TracingDataCommandSender("http://monitoring-service:8080"));
        services.AddSingleton<ITraceCollector, TraceCollector>();
    }

    private static void AddTicketTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ICreateTicketTraceCollector, CreateTicketTraceCollector>();
        services.AddSingleton<IUpdateTicketTraceCollector, UpdateTicketTraceCollector>();

        services.AddSingleton<ITicketUseCaseSelector, TicketUseCaseSelector>();

        services.AddSingleton<ICreateSprintTraceCollector, CreateSprintTraceCollector>();
        services.AddSingleton<ISprintActiveStatusCollector, SprintActiveStatusCollector>();
        services.AddSingleton<ITicketAddedToSprintCollector, TicketAddedToSprintCollector>();
        services.AddSingleton<IUpdateSprintCollector, UpdateSprintCollector>();

        services.AddSingleton<ISprintUseCaseSelector, SprintUseCaseSelector>();

        services.AddSingleton<ICreateTagTraceCollector, CreateTagTraceCollector>();
        services.AddSingleton<IUpdateTagTraceCollector, UpdateTagTraceCollector>();

        services.AddSingleton<ITagUseCaseSelector, TagUseCaseSelector>();

        services.AddSingleton<ICreateNoteTypeTraceCollector, CreateNoteTypeTraceCollector>();
        services.AddSingleton<IChangeNoteTypeColorTraceCollector, ChangeNoteTypeColorTraceCollector>();
        services.AddSingleton<IChangeNoteTypeNameTraceCollector, ChangeNoteTypeNameTraceCollector>();

        services.AddSingleton<INoteTypeUseCaseSelector, NoteTypeUseCaseSelector>();


        services.AddSingleton<ICreateTimerSettingsTraceCollector, CreateTimerSettingsTraceCollector>();
        services.AddSingleton<IChangeDocuTimerSaveIntervalTraceCollector, ChangeDocuTimerSaveIntervalTraceCollector>();
        services.AddSingleton<IChangeSnapshotSaveIntervalTraceCollector, ChangeSnapshotSaveIntervalTraceCollector>();

        services.AddSingleton<ITimerSettingsUseCaseSelector, TimerSettingsUseCaseSelector>();
    }
}