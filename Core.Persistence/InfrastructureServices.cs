using Core.Persistence.DbContext;
using Core.Persistence.EventStores;
using Domain.Events.Note;
using Domain.Events.NoteTypes;
using Domain.Events.Sprints;
using Domain.Events.StatisticsData;
using Domain.Events.Tags;
using Domain.Events.TimerSettings;
using Domain.Events.TimeSlots;
using Domain.Events.WorkDays;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Persistence;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        AddDatabaseServices(services);
    }

    private static void AddDatabaseServices(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("./appsettings.json", false, true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IEventStore<NoteEvent>, NoteEventsStore>();
        services.AddScoped<IEventStore<NoteTypeEvent>, NoteTypeEventsStore>();
        services.AddScoped<ITicketEventsStore, TicketEventsStore>();
        services.AddScoped<IEventStore<SprintEvent>, SprintEventsStore>();
        services.AddScoped<IEventStore<StatisticsDataEvent>, StatisticsDataEventsStore>();
        services.AddScoped<IEventStore<TimeSlotEvent>, TimeSlotEventsStore>();
        services.AddScoped<IEventStore<TagEvent>, TagEventsStore>();
        services.AddScoped<IEventStore<WorkDayEvent>, WorkDayEventsStore>();
        services.AddScoped<IEventStore<TimerSettingsEvent>, TimerSettingsEventsStore>();
    }
}