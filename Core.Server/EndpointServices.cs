using Core.Server.Communication.Endpoints.Analysis;
using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Endpoints.NoteType;
using Core.Server.Communication.Endpoints.Sprint;
using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TicketReplay;
using Core.Server.Communication.Endpoints.TimerSettings;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Endpoints.UseCase;
using Core.Server.Communication.Endpoints.WorkDay;
using Core.Server.Communication.Mocks.Analysis;
using Core.Server.Communication.Mocks.Note;
using Core.Server.Communication.Mocks.NoteType;
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
        app.MapGrpcService<NoteCommandReceiver>();
        app.MapGrpcService<NoteTypeCommandReceiver>();
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
        app.MapGrpcService<NoteRequestReceiver>();
        app.MapGrpcService<NoteTypeRequestReceiver>();
        app.MapGrpcService<SprintRequestReceiver>();
        app.MapGrpcService<StatisticsDataRequestReceiver>();
        app.MapGrpcService<TagRequestReceiver>();
        app.MapGrpcService<TicketRequestReceiver>();
        app.MapGrpcService<TicketReplayRequestReceiver>();
        app.MapGrpcService<TimerSettingsRequestReceiver>();
        app.MapGrpcService<TimeSlotRequestReceiver>();
        app.MapGrpcService<WorkDayRequestReceiver>();
        app.MapGrpcService<UseCaseRequestReceiver>();
        app.MapGrpcService<AnalysisRequestReceiver>();
    }

    private static void AddNotificationEndPoints(WebApplication app)
    {
        app.MapGrpcService<NoteNotificationService>();
        app.MapGrpcService<NoteTypeNotificationService>();
        app.MapGrpcService<SprintNotificationService>();
        app.MapGrpcService<StatisticsDataNotificationService>();
        app.MapGrpcService<TagNotificationService>();
        app.MapGrpcService<TicketNotificationService>();
        app.MapGrpcService<TimerSettingsNotificationService>();
        app.MapGrpcService<TimeSlotNotificationService>();
        app.MapGrpcService<UseCaseNotificationService>();
        app.MapGrpcService<WorkDayNotificationService>();
    }

    public static void AddMockingEndpoints(this WebApplication app)
    {
        app.MapGrpcService<MockNoteCommandReceiver>();
        app.MapGrpcService<MockNoteTypeCommandService>();
        app.MapGrpcService<MockSprintCommandService>();
        app.MapGrpcService<MockStatisticsDataCommandService>();
        app.MapGrpcService<MockTagCommandService>();
        app.MapGrpcService<MockTicketCommandService>();
        app.MapGrpcService<MockTimerSettingsCommandService>();
        app.MapGrpcService<MockTimeSlotCommandService>();
        app.MapGrpcService<MockTraceReportCommandService>();
        app.MapGrpcService<MockUseCaseCommandService>();
        app.MapGrpcService<MockWorkDayCommandService>();

        app.MapGrpcService<NoteRequestReceiver>();
        app.MapGrpcService<MockNoteTypeRequestService>();
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