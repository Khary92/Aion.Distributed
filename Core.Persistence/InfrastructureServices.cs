using Core.Persistence.SQLite.DbContext;
using Core.Persistence.SQLite.EventStores;
using Domain.Events.AiSettings;
using Domain.Events.Note;
using Domain.Events.NoteTypes;
using Domain.Events.Settings;
using Domain.Events.Sprints;
using Domain.Events.StatisticsData;
using Domain.Events.Tags;
using Domain.Events.TimerSettings;
using Domain.Events.TimeSlots;
using Domain.Events.WorkDays;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Persistence;

public static class InfrastructureServices
{
    private const string DatabaseName = "events.db";

    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        AddDatabaseServices(services);
    }

    private static void AddDatabaseServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={DatabaseName}"));
        services.AddDbContextFactory<AppDbContext>();

        services.AddScoped<IEventStore<AiSettingsEvent>, AiSettingsEventsStore>();
        services.AddScoped<IEventStore<NoteEvent>, NoteEventsStore>();
        services.AddScoped<IEventStore<NoteTypeEvent>, NoteTypeEventsStore>();
        services.AddScoped<ITicketEventsStore, TicketEventsStore>();
        services.AddScoped<IEventStore<SprintEvent>, SprintEventsStore>();
        services.AddScoped<IEventStore<StatisticsDataEvent>, StatisticsDataEventsStore>();
        services.AddScoped<IEventStore<TimeSlotEvent>, TimeSlotEventsStore>();
        services.AddScoped<IEventStore<TagEvent>, TagEventsStore>();
        services.AddScoped<IEventStore<WorkDayEvent>, WorkDayEventsStore>();
        services.AddScoped<IEventStore<SettingsEvent>, SettingsEventsStore>();
        services.AddScoped<IEventStore<TimerSettingsEvent>, TimerSettingsEventsStore>();
    }
}