using Core.Server.Communication.Endpoints.Analysis;
using Core.Server.Communication.Endpoints.Client;
using Core.Server.Communication.Endpoints.Note;
using Core.Server.Communication.Endpoints.NoteType;
using Core.Server.Communication.Endpoints.Sprint;
using Core.Server.Communication.Endpoints.StatisticsData;
using Core.Server.Communication.Endpoints.Tag;
using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Endpoints.TicketReplay;
using Core.Server.Communication.Endpoints.TimerSettings;
using Core.Server.Communication.Endpoints.TimeSlot;
using Core.Server.Communication.Endpoints.WorkDay;

namespace Core.Server;

public static class EndpointServices
{
    private const string AuthorizationScheme = "InternalOrAuthenticated";

    public static void AddEndPoints(this WebApplication app)
    {
        AddCommandEndPoints(app);
        AddRequestEndPoints(app);
        AddNotificationEndPoints(app);
    }

    private static void AddCommandEndPoints(WebApplication app)
    {
        app.MapGrpcService<NoteCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<NoteTypeCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<SprintCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<StatisticsDataCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TagCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TicketCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TimerSettingsCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TimeSlotCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<ClientCommandReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<WorkDayCommandReceiver>().RequireAuthorization(AuthorizationScheme);
    }

    private static void AddRequestEndPoints(WebApplication app)
    {
        app.MapGrpcService<NoteRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<NoteTypeRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<SprintRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<StatisticsDataRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TagRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TicketRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TicketReplayRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TimerSettingsRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TimeSlotRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<WorkDayRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<UseCaseRequestReceiver>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<AnalysisRequestReceiver>().RequireAuthorization(AuthorizationScheme);
    }

    private static void AddNotificationEndPoints(WebApplication app)
    {
        app.MapGrpcService<NoteNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<NoteTypeNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<SprintNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<StatisticsDataNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TagNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TicketNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TimerSettingsNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<TimeSlotNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<UseCaseNotificationService>().RequireAuthorization(AuthorizationScheme);
        app.MapGrpcService<WorkDayNotificationService>().RequireAuthorization(AuthorizationScheme);
    }
}