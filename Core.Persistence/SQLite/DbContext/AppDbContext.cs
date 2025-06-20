using Domain.Events.AiSettings;
using Domain.Events.Note;
using Domain.Events.NoteTypes;
using Domain.Events.Settings;
using Domain.Events.Sprints;
using Domain.Events.StatisticsData;
using Domain.Events.Tags;
using Domain.Events.Tickets;
using Domain.Events.TimerSettings;
using Domain.Events.TimeSlots;
using Domain.Events.WorkDays;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.SQLite.DbContext;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    private const string DatabaseName = "sqlite.db";

    public AppDbContext() : this(null!)
    {
        Database.Migrate();
    }

    public DbSet<TicketEvent> TicketEvents { get; set; }
    public DbSet<SprintEvent> SprintEvents { get; set; }
    public DbSet<StatisticsDataEvent> StatisticsDataEvents { get; set; }
    public DbSet<TimeSlotEvent> TimeSlotEvents { get; set; }
    public DbSet<WorkDayEvent> WorkDayEvents { get; set; }
    public DbSet<NoteEvent> NoteEvents { get; set; }
    public DbSet<NoteTypeEvent> NoteTypeEvents { get; set; }
    public DbSet<TagEvent> TagEvents { get; set; }
    public DbSet<SettingsEvent> SettingsEvents { get; set; }
    public DbSet<AiSettingsEvent> AiSettingsEvents { get; set; }
    public DbSet<TimerSettingsEvent> TimerSettingsEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.UseSqlite($"Data Source={DatabaseName}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TicketEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<SprintEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<StatisticsDataEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<TimeSlotEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<WorkDayEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<TagEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<SettingsEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<AiSettingsEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<NoteEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<NoteTypeEvent>()
            .HasKey(te => te.EventId);

        modelBuilder.Entity<TimerSettingsEvent>()
            .HasKey(te => te.EventId);
    }
}