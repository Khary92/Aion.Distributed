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

var app = builder.Build();

// Command Services registrieren
app.MapGrpcService<AiSettingsCommandServiceImpl>();
app.MapGrpcService<NoteCommandServiceImpl>();
app.MapGrpcService<NoteTypeCommandServiceImpl>();
app.MapGrpcService<SettingsCommandServiceImpl>();
app.MapGrpcService<SprintCommandServiceImpl>();
app.MapGrpcService<StatisticsDataCommandServiceImpl>();
app.MapGrpcService<TagCommandServiceImpl>();
app.MapGrpcService<TicketCommandServiceImpl>();
app.MapGrpcService<TimerSettingsCommandServiceImpl>();
app.MapGrpcService<TimeSlotCommandServiceImpl>();
app.MapGrpcService<TraceReportCommandServiceImpl>();
app.MapGrpcService<UseCaseCommandServiceImpl>();
app.MapGrpcService<WorkDayCommandServiceImpl>();

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
app.MapGrpcService<AiSettingsRequestServiceImpl>();
app.MapGrpcService<NoteRequestServiceImpl>();
app.MapGrpcService<NoteTypeRequestServiceImpl>();
app.MapGrpcService<SettingsRequestServiceImpl>();
app.MapGrpcService<SprintRequestServiceImpl>();
app.MapGrpcService<StatisticsDataRequestServiceImpl>();
app.MapGrpcService<TagRequestServiceImpl>();
app.MapGrpcService<TicketRequestServiceImpl>();
app.MapGrpcService<TimerSettingsRequestServiceImpl>();
app.MapGrpcService<TimeSlotRequestServiceImpl>();
app.MapGrpcService<WorkDayRequestServiceImpl>();

// TODO remove in productive environment
app.MapGrpcReflectionService();

app.Run();