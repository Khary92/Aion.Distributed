using Microsoft.AspNetCore.Server.Kestrel.Core;
using Service.Server.Communication.UseCase;
using Service.Server.Mock.AiSettings;
using Service.Server.Mock.Analysis;
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

namespace Service.Server;

public static class BootStrapper
{
    public static WebApplication BuildWebApp(String[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options => { options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2); });
        AddNotificationServices(builder.Services);
        return builder.Build();
    }
    
    private static void AddNotificationServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        
        services.AddSingleton<AiSettingsNotificationServiceImpl>();
        services.AddSingleton<NoteNotificationServiceImpl>();
        services.AddSingleton<NoteTypeNotificationServiceImpl>();
        services.AddSingleton<SettingsNotificationServiceImpl>();
        services.AddSingleton<SprintNotificationServiceImpl>();
        services.AddSingleton<StatisticsDataNotificationServiceImpl>();
        services.AddSingleton<TagNotificationServiceImpl>();
        services.AddSingleton<TicketNotificationServiceImpl>();
        services.AddSingleton<TimerSettingsNotificationServiceImpl>();
        services.AddSingleton<TimeSlotNotificationServiceImpl>();
        services.AddSingleton<TraceReportNotificationServiceImpl>();
        services.AddSingleton<UseCaseNotificationServiceImpl>();
        services.AddSingleton<WorkDayNotificationServiceImpl>();
    }

    public static void AddEndPoints(this WebApplication app)
    {
        AddCommandEndPoints(app);
    }
    
    private static void AddCommandEndPoints(WebApplication app)
    {
        
    }

    public static void AddNotificationEndPoints(WebApplication app)
    {
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
    }

    public static void AddMockingEndpoints(this WebApplication app)
    {
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
        app.MapGrpcService<MockUseCaseRequestService>();
        app.MapGrpcService<MockAnalysisRequestService>();
    }
}