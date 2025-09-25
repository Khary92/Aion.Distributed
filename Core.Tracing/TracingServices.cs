using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Tracing.Tracing.Tracers.Client;
using Core.Server.Tracing.Tracing.Tracers.Client.UseCase;
using Core.Server.Tracing.Tracing.Tracers.Note;
using Core.Server.Tracing.Tracing.Tracers.Note.UseCase;
using Core.Server.Tracing.Tracing.Tracers.NoteType;
using Core.Server.Tracing.Tracing.Tracers.NoteType.UseCase;
using Core.Server.Tracing.Tracing.Tracers.Sprint;
using Core.Server.Tracing.Tracing.Tracers.Sprint.UseCase;
using Core.Server.Tracing.Tracing.Tracers.Statistics;
using Core.Server.Tracing.Tracing.Tracers.Statistics.UseCase;
using Core.Server.Tracing.Tracing.Tracers.Tag;
using Core.Server.Tracing.Tracing.Tracers.Tag.UseCase;
using Core.Server.Tracing.Tracing.Tracers.Ticket;
using Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;
using Core.Server.Tracing.Tracing.Tracers.TimerSettings;
using Core.Server.Tracing.Tracing.Tracers.TimerSettings.UseCase;
using Core.Server.Tracing.Tracing.Tracers.TimeSlot;
using Core.Server.Tracing.Tracing.Tracers.TimeSlot.UseCase;
using Core.Server.Tracing.Tracing.Tracers.WorkDay;
using Core.Server.Tracing.Tracing.Tracers.WorkDay.UseCase;
using Microsoft.Extensions.DependencyInjection;
using Service.Monitoring.Shared.Tracing;
using CreateNoteTraceCollector = Core.Server.Tracing.Tracing.Tracers.Note.UseCase.CreateNoteTraceCollector;
using NoteUseCaseSelector = Core.Server.Tracing.Tracing.Tracers.Note.NoteUseCaseSelector;

namespace Core.Server.Tracing;

public static class TracingServices
{
    public static void AddTracingServices(this IServiceCollection services)
    {
        AddACommonTracingServices(services);
        AddNoteTracingServices(services);
        AddNoteTypeTracingServices(services);
        AddSprintTracingServices(services);
        AddStatisticsDataTracingServices(services);
        AddTagTracingServices(services);
        AddTicketTracingServices(services);
        AddTimerSettingsTracingServices(services);
        AddTimeSlotTracingServices(services);
        AddWorkDayTracingServices(services);
        AddClientTracingServices(services);
    }
    
    private static void AddWorkDayTracingServices(IServiceCollection services)
    {
        services.AddSingleton<ICreateWorkDayTraceCollector, CreateWorkDayTraceCollector>();

        services.AddSingleton<IWorkDayUseCaseSelector, WorkDayUseCaseSelector>();
    }

    private static void AddACommonTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<ITracingDataSender>(_ =>
            new TracingDataSender("http://monitoring-service:8080"));
        services.AddSingleton<ITraceCollector, TraceCollector>();
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

    private static void AddStatisticsDataTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<IChangeProductivityTraceCollector, ChangeProductivityTraceCollector>();
        services.AddSingleton<IChangeTagSelectionTraceCollector, ChangeTagSelectionTraceCollector>();

        services.AddSingleton<IStatisticsDataUseCaseSelector, StatisticsDataUseCaseSelector>();
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
        services.AddSingleton<IUpdateTicketTraceCollector, UpdateTicketTraceCollector>();

        services.AddSingleton<ITicketUseCaseSelector, TicketUseCaseSelector>();
    }

    private static void AddTimerSettingsTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<IChangeDocuTimerSaveIntervalTraceCollector, ChangeDocuTimerSaveIntervalTraceCollector>();
        services.AddSingleton<IChangeSnapshotSaveIntervalTraceCollector, ChangeSnapshotSaveIntervalTraceCollector>();

        services.AddSingleton<ITimerSettingsUseCaseSelector, TimerSettingsUseCaseSelector>();
    }

    private static void AddTimeSlotTracingServices(this IServiceCollection services)
    {
        services.AddSingleton<IAddNoteTraceCollector, AddNoteTraceCollector>();
        services.AddSingleton<ISetStartTimeTraceCollector, SetStartTimeTraceCollector>();
        services.AddSingleton<ISetEndTimeTraceCollector, SetEndTimeTraceCollector>();

        services.AddSingleton<ITimeSlotUseCaseSelector, TimeSlotUseCaseSelector>();
    }
    
    private static void AddClientTracingServices(IServiceCollection services)
    {
        services.AddSingleton<ICreateTrackingControlCollector, CreateTrackingControlCollector>();

        services.AddSingleton<IClientUseCaseSelector, ClientUseCaseSelector>();
    }

}