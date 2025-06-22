using Core.Server.Communication.Mock.AiSettings;
using Core.Server.Communication.Mock.Analysis;
using Core.Server.Communication.Mock.Note;
using Core.Server.Communication.Mock.NoteType;
using Core.Server.Communication.Mock.Settings;
using Core.Server.Communication.Mock.Sprint;
using Core.Server.Communication.Mock.StatisticsData;
using Core.Server.Communication.Mock.Tag;
using Core.Server.Communication.Mock.Ticket;
using Core.Server.Communication.Mock.TimerSettings;
using Core.Server.Communication.Mock.TimeSlot;
using Core.Server.Communication.Mock.TraceReport;
using Core.Server.Communication.Mock.UseCase;
using Core.Server.Communication.Mock.WorkDay;
using Core.Server.Communication.Services.AiSettings;
using Core.Server.Communication.Services.Note;
using Core.Server.Communication.Services.NoteType;
using Core.Server.Communication.Services.Settings;
using Core.Server.Communication.Services.Sprint;
using Core.Server.Communication.Services.StatisticsData;
using Core.Server.Communication.Services.Tag;
using Core.Server.Communication.Services.Ticket;
using Core.Server.Communication.Services.TimerSettings;
using Core.Server.Communication.Services.TimeSlot;
using Core.Server.Communication.Services.UseCase;
using Core.Server.Communication.Services.WorkDay;
using Core.Server.Services.Entities.TimeSlots;

namespace Core.Server;

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
        app.MapGrpcService<UseCaseRequestReceiver>();

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