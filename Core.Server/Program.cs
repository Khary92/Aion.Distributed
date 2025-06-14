using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Server.Mock.AiSettings;
using Service.Server.Mock.Note;
using Service.Server.Mock.NoteType;
using Service.Server.Mock.Settings;
using Service.Server.Mock.Sprint;
using Service.Server.Mock.StatisticsData;
using Service.Server.Mock.Tag;
using Service.Server.Mock.Ticket;
using Service.Server.Mock.TimerSettings;
using Service.Server.Mock.TimeSlot;
using Service.Server.Mock.TraceReport;
using Service.Server.Mock.UseCase;
using Service.Server.Mock.WorkDay;

var builder = WebApplication.CreateBuilder(args);

// gRPC Services und Reflection registrieren
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.WebHost.ConfigureKestrel(options => { options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2); });

// Notification Services als Singleton registrieren
builder.Services.AddSingleton<AiSettingsNotificationServiceImpl>();
builder.Services.AddSingleton<NoteNotificationServiceImpl>();
builder.Services.AddSingleton<NoteTypeNotificationServiceImpl>();
builder.Services.AddSingleton<SettingsNotificationServiceImpl>();
builder.Services.AddSingleton<SprintNotificationServiceImpl>();
builder.Services.AddSingleton<StatisticsDataNotificationServiceImpl>();
builder.Services.AddSingleton<TagNotificationServiceImpl>();
builder.Services.AddSingleton<TicketNotificationServiceImpl>();
builder.Services.AddSingleton<TimerSettingsNotificationServiceImpl>();
builder.Services.AddSingleton<TimeSlotNotificationServiceImpl>();
builder.Services.AddSingleton<TraceReportNotificationServiceImpl>();
builder.Services.AddSingleton<UseCaseNotificationServiceImpl>();
builder.Services.AddSingleton<WorkDayNotificationServiceImpl>();

var app = builder.Build();

// Command Services registrieren
app.MapGrpcService<MockAiSettingsCommandService>();
app.MapGrpcService<MockNoteCommandService>();
app.MapGrpcService<MockNoteTypeCommandService>();
app.MapGrpcService<MockSettingsCommandService>();
app.MapGrpcService<MockSprintCommandService>();
app.MapGrpcService<MockStatisticsDataCommandService>();
app.MapGrpcService<MockCommandService>();
app.MapGrpcService<MockTicketCommandService>();
app.MapGrpcService<MockTimerSettingsCommandService>();
app.MapGrpcService<MockTimeSlotCommandService>();
app.MapGrpcService<MockTraceReportCommandService>();
app.MapGrpcService<MockUseCaseCommandService>();
app.MapGrpcService<MockWorkDayCommandService>();

// Notification Services registrieren
app.MapGrpcService<AiSettingsNotificationServiceImpl>();
app.MapGrpcService<NoteNotificationServiceImpl>();
app.MapGrpcService<NoteTypeNotificationServiceImpl>();
app.MapGrpcService<SettingsNotificationServiceImpl>();
app.MapGrpcService<SprintNotificationServiceImpl>();
app.MapGrpcService<StatisticsDataNotificationServiceImpl>();
app.MapGrpcService<TagNotificationServiceImpl>();
app.MapGrpcService<TicketNotificationServiceImpl>();
app.MapGrpcService<TimerSettingsNotificationServiceImpl>();
app.MapGrpcService<TimeSlotNotificationServiceImpl>();
app.MapGrpcService<TraceReportNotificationServiceImpl>();
app.MapGrpcService<UseCaseNotificationServiceImpl>();
app.MapGrpcService<WorkDayNotificationServiceImpl>();

// Request Services registrieren
app.MapGrpcService<MockAiSettingsRequestService>();
app.MapGrpcService<MockNoteRequestService>();
app.MapGrpcService<MockNoteTypeRequestService>();
app.MapGrpcService<MockSettingsRequestService>();
app.MapGrpcService<MockSprintRequestService>();
app.MapGrpcService<MockStatisticsDataRequestService>();
app.MapGrpcService<MockTagRequestService>();
app.MapGrpcService<MockTicketRequestService>();
app.MapGrpcService<MockTimerSettingsRequestService>();
app.MapGrpcService<MockTimeSlotRequestService>();
app.MapGrpcService<MockWorkDayRequestService>();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
    foreach (var endpoint in endpointDataSource.Endpoints)
    {
        Console.WriteLine($"[Endpoint] {endpoint.DisplayName}");
    }
});


// TODO remove in productive environment
app.MapGrpcReflectionService();

app.Run();