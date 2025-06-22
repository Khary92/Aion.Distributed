using Core.Server.Communication.Endpoints.AiSettings;
using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Endpoints.NoteType;
using Core.Server.Communication.Endpoints.Settings;
using Core.Server.Communication.Endpoints.Sprint;
using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TimerSettings;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Endpoints.UseCase;
using Core.Server.Communication.Endpoints.WorkDay;
using Core.Server.Communication.Mocks.AiSettings;
using Core.Server.Communication.Mocks.Analysis;
using Core.Server.Communication.Mocks.Note;
using Core.Server.Communication.Mocks.NoteType;
using Core.Server.Communication.Mocks.Settings;
using Core.Server.Communication.Mocks.Sprint;
using Core.Server.Communication.Mocks.StatisticsData;
using Core.Server.Communication.Mocks.Tag;
using Core.Server.Communication.Mocks.Ticket;
using Core.Server.Communication.Mocks.TimerSettings;
using Core.Server.Communication.Mocks.TimeSlot;
using Core.Server.Communication.Mocks.TraceReport;
using Core.Server.Communication.Mocks.UseCase;
using Core.Server.Communication.Mocks.WorkDay;

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
        app.MapGrpcService<TimeSlotCommandReceiver>();
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