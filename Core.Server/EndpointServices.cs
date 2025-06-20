using Service.Server.Communication.Mock.AiSettings;
using Service.Server.Communication.Mock.Analysis;
using Service.Server.Communication.Mock.Note;
using Service.Server.Communication.Mock.NoteType;
using Service.Server.Communication.Mock.Settings;
using Service.Server.Communication.Mock.Sprint;
using Service.Server.Communication.Mock.StatisticsData;
using Service.Server.Communication.Mock.Tag;
using Service.Server.Communication.Mock.Ticket;
using Service.Server.Communication.Mock.TimerSettings;
using Service.Server.Communication.Mock.TimeSlot;
using Service.Server.Communication.Mock.TraceReport;
using Service.Server.Communication.Mock.UseCase;
using Service.Server.Communication.Mock.WorkDay;
using Service.Server.Communication.Services.AiSettings;
using Service.Server.Communication.Services.Note;
using Service.Server.Communication.Services.NoteType;
using Service.Server.Communication.Services.Settings;
using Service.Server.Communication.Services.Sprint;
using Service.Server.Communication.Services.StatisticsData;
using Service.Server.Communication.Services.Tag;
using Service.Server.Communication.Services.Ticket;
using Service.Server.Communication.Services.TimerSettings;
using Service.Server.Communication.Services.TimeSlot;
using Service.Server.Communication.Services.UseCase;
using Service.Server.Communication.Services.WorkDay;
using Service.Server.Services.Entities.TimeSlots;

namespace Service.Server;

public static class EndpointServices
{
    
    public static void AddEndPoints(this WebApplication app)
    {
        AddCommandEndPoints(app);
        AddRequestEndPoints(app);
        AddNotificationEndPoints(app);
    }

    private static void AddCommandEndPoints(WebApplication app)
    {
        app.MapGrpcService<AiSettingsCommandReceiver>();
        app.MapGrpcService<NoteCommandReceiver>();
        app.MapGrpcService<NoteTypeCommandReceiver>();
        app.MapGrpcService<SettingsCommandReceiver>();
        app.MapGrpcService<SprintCommandReceiver>();
        app.MapGrpcService<StatisticsDataCommandReceiver>();
        app.MapGrpcService<TagCommandReceiver>();
        app.MapGrpcService<TicketCommandReceiver>();
        app.MapGrpcService<TimerSettingsCommandReceiver>();
        app.MapGrpcService<TimeSlotCommandService>();
        app.MapGrpcService<UseCaseCommandReceiver>();
        app.MapGrpcService<WorkDayCommandReceiver>();
    }

    private static void AddRequestEndPoints(WebApplication app)
    {
        app.MapGrpcService<AiSettingsRequestReceiver>();
        app.MapGrpcService<NoteRequestReceiver>();
        app.MapGrpcService<NoteTypeRequestReceiver>();
        app.MapGrpcService<SettingsRequestReceiver>();
        app.MapGrpcService<SprintRequestReceiver>();
        app.MapGrpcService<StatisticsDataRequestReceiver>();
        app.MapGrpcService<TagRequestReceiver>();
        app.MapGrpcService<TicketRequestReceiver>();
        app.MapGrpcService<TimerSettingsRequestReceiver>();
        app.MapGrpcService<TimeSlotRequestReceiver>();
        app.MapGrpcService<WorkDayRequestReceiver>();

        //TODO
        //app.MapGrpcService<UseCaseRequestReceiver>();
        //app.MapGrpcService<AnalysisRequestReceiver>();
    }

    public static void AddNotificationEndPoints(WebApplication app)
    {
        app.MapGrpcService<AiSettingsNotificationService>();
        app.MapGrpcService<NoteNotificationService>();
        app.MapGrpcService<NoteTypeNotificationService>();
        app.MapGrpcService<SettingsNotificationService>();
        app.MapGrpcService<SprintNotificationService>();
        app.MapGrpcService<StatisticsDataNotificationService>();
        app.MapGrpcService<TagNotificationServiceImpl>();
        app.MapGrpcService<TicketNotificationService>();
        app.MapGrpcService<TimerSettingsNotificationService>();
        app.MapGrpcService<TimeSlotNotificationService>();
        app.MapGrpcService<UseCaseNotificationService>();
        app.MapGrpcService<WorkDayNotificationService>();
    }

    public static void AddMockingEndpoints(this WebApplication app)
    {
        app.MapGrpcService<MockAiSettingsCommandReceiver>();
        app.MapGrpcService<MockNoteCommandReceiver>();
        app.MapGrpcService<MockNoteTypeCommandService>();
        app.MapGrpcService<MockSettingsCommandService>();
        app.MapGrpcService<MockSprintCommandService>();
        app.MapGrpcService<MockStatisticsDataCommandService>();
        app.MapGrpcService<MockTagCommandService>();
        app.MapGrpcService<MockTicketCommandService>();
        app.MapGrpcService<MockTimerSettingsCommandService>();
        app.MapGrpcService<MockTimeSlotCommandService>();
        app.MapGrpcService<MockTraceReportCommandService>();
        app.MapGrpcService<MockUseCaseCommandService>();
        app.MapGrpcService<MockWorkDayCommandService>();

        app.MapGrpcService<MockAiSettingsRequestReceiver>();
        app.MapGrpcService<NoteRequestReceiver>();
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