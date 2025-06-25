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

public sealed class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<TicketEvent> TicketEvents { get; set; } = null!;
    public DbSet<SprintEvent> SprintEvents { get; set; } = null!;
    public DbSet<StatisticsDataEvent> StatisticsDataEvents { get; set; } = null!;
    public DbSet<TimeSlotEvent> TimeSlotEvents { get; set; } = null!;
    public DbSet<WorkDayEvent> WorkDayEvents { get; set; } = null!;
    public DbSet<NoteEvent> NoteEvents { get; set; } = null!;
    public DbSet<NoteTypeEvent> NoteTypeEvents { get; set; } = null!;
    public DbSet<TagEvent> TagEvents { get; set; } = null!;
    public DbSet<SettingsEvent> SettingsEvents { get; set; } = null!;
    public DbSet<AiSettingsEvent> AiSettingsEvents { get; set; } = null!;
    public DbSet<TimerSettingsEvent> TimerSettingsEvents { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseNpgsql(DatabaseConfiguration.GetConnectionString());
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