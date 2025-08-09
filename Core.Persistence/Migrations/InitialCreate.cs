#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "NoteEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_NoteEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "NoteTypeEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_NoteTypeEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "SprintEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_SprintEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "StatisticsDataEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_StatisticsDataEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "TagEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_TagEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "TicketEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_TicketEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "TimerSettingsEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_TimerSettingsEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "TimeSlotEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_TimeSlotEvents", x => x.EventId); });

        migrationBuilder.CreateTable(
            "WorkDayEvents",
            table => new
            {
                EventId = table.Column<Guid>("uuid", nullable: false),
                TimeStamp = table.Column<DateTime>("timestamp with time zone", nullable: false),
                Offset = table.Column<TimeSpan>("interval", nullable: false),
                EventType = table.Column<string>("text", nullable: false),
                EntityId = table.Column<Guid>("uuid", nullable: false),
                EventPayload = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_WorkDayEvents", x => x.EventId); });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "NoteEvents");

        migrationBuilder.DropTable(
            "NoteTypeEvents");

        migrationBuilder.DropTable(
            "SprintEvents");

        migrationBuilder.DropTable(
            "StatisticsDataEvents");

        migrationBuilder.DropTable(
            "TagEvents");

        migrationBuilder.DropTable(
            "TicketEvents");

        migrationBuilder.DropTable(
            "TimerSettingsEvents");

        migrationBuilder.DropTable(
            "TimeSlotEvents");

        migrationBuilder.DropTable(
            "WorkDayEvents");
    }
}