using Application.Cache;
using Application.Handler.Commands.Entities.AiSettings;
using Application.Handler.Commands.Entities.Note;
using Application.Handler.Commands.Entities.NoteType;
using Application.Handler.Commands.Entities.Settings;
using Application.Handler.Commands.Entities.Sprints;
using Application.Handler.Commands.Entities.StatisticsData;
using Application.Handler.Commands.Entities.Tags;
using Application.Handler.Commands.Entities.Tickets;
using Application.Handler.Commands.Entities.TimerSettings;
using Application.Handler.Commands.Entities.TimeSlots;
using Application.Handler.Commands.Entities.WorkDays;
using Application.Handler.Requests.AiSettings;
using Application.Handler.Requests.Notes;
using Application.Handler.Requests.NoteTypes;
using Application.Handler.Requests.Replays;
using Application.Handler.Requests.Settings;
using Application.Handler.Requests.Sprints;
using Application.Handler.Requests.StatisticsData;
using Application.Handler.Requests.Tags;
using Application.Handler.Requests.Tickets;
using Application.Handler.Requests.TimerSettings;
using Application.Handler.Requests.TimeSlots;
using Application.Handler.Requests.WorkDays;
using Application.Mapper;
using Application.Services.Entities.AiSettings;
using Application.Services.Entities.Notes;
using Application.Services.Entities.NoteTypes;
using Application.Services.Entities.Settings;
using Application.Services.Entities.Sprints;
using Application.Services.Entities.StatisticsData;
using Application.Services.Entities.Tags;
using Application.Services.Entities.Tickets;
using Application.Services.Entities.TimerSettings;
using Application.Services.Entities.TimeSlots;
using Application.Services.Entities.WorkDays;
using Application.Services.UseCase;
using Application.Services.UseCase.Replays;
using Application.Translators.AiSettings;
using Application.Translators.Notes;
using Application.Translators.NoteTypes;
using Application.Translators.Settings;
using Application.Translators.Sprints;
using Application.Translators.StatisticsData;
using Application.Translators.Tags;
using Application.Translators.Tickets;
using Application.Translators.TimerSettings;
using Application.Translators.TimeSlots;
using Application.Translators.WorkDays;
using SendTraceReportCommandHandler = Application.Handler.Commands.Tracing.SendTraceReportCommandHandler;

namespace Application;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        AddMapperServices(services);
        AddCommonServices(services);
        AddRequestsServices(services);
        AddCommandsServices(services);
        AddCommandToEventTranslators(services);
        AddCommandToNotificationTranslators(services);
        AddRequestHandlers(services);
        AddCommandHandlers(services);
    }

    private static void AddMapperServices(this IServiceCollection services)
    {
        services.AddSingleton<IDtoMapper<SettingsDto, Settings>, SettingsMapper>();
        services.AddSingleton<IDtoMapper<AiSettingsDto, AiSettings>, AiSettingsMapper>();
        services.AddSingleton<IDtoMapper<NoteDto, Note>, NoteMapper>();
        services.AddSingleton<IDtoMapper<NoteTypeDto, NoteType>, NoteTypeMapper>();
        services.AddSingleton<IDtoMapper<SprintDto, Sprint>, SprintMapper>();
        services.AddSingleton<IDtoMapper<StatisticsDataDto, StatisticsData>, StatisticsDataMapper>();
        services.AddSingleton<IDtoMapper<TagDto, Tag>, TagMapper>();
        services.AddSingleton<IDtoMapper<TicketDto, Ticket>, TicketMapper>();
        services.AddSingleton<IDtoMapper<TimeSlotDto, TimeSlot>, TimeSlotMapper>();
        services.AddSingleton<IDtoMapper<WorkDayDto, WorkDay>, WorkDayMapper>();
        services.AddSingleton<IDtoMapper<TimerSettingsDto, TimerSettings>, TimerSettingsMapper>();
    }

    private static void AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
        services.AddSingleton<IRunTimeSettings, RunTimeSettings>();
        services.AddSingleton<IReplayRequestsService, ReplayRequestsService>();
        services.AddSingleton<IPersistentCache<SetStartTimeCommand>, StartTimeChangedCache>();
        services.AddSingleton<IPersistentCache<SetEndTimeCommand>, EndTimeChangedCache>();

        services.AddTransient<ITracingCollectorProvider, TracingCollectorProvider>();
        services.AddSingleton<TimerService>();
        services.AddScoped<INotificationHandler<TimerSettingsCreatedNotification>>(sp =>
            sp.GetRequiredService<TimerService>());
        services.AddScoped<INotificationHandler<DocuTimerSaveIntervalChangedNotification>>(sp =>
            sp.GetRequiredService<TimerService>());
        services.AddScoped<INotificationHandler<SnapshotSaveIntervalChangedNotification>>(sp =>
            sp.GetRequiredService<TimerService>());
    }

    private static void AddRequestsServices(this IServiceCollection services)
    {
        services.AddSingleton<IAiSettingsRequestsService, AiSettingsRequestsService>();
        services.AddSingleton<INoteRequestsService, NoteRequestsService>();
        services.AddSingleton<INoteTypeRequestsService, NoteTypeRequestsService>();
        services.AddSingleton<ISprintRequestsService, SprintRequestService>();
        services.AddSingleton<ITicketRequestsService, TicketRequestService>();
        services.AddSingleton<IWorkDayRequestsService, WorkDayRequestsService>();
        services.AddSingleton<ITagRequestsService, TagRequestsService>();
        services.AddSingleton<ITimeSlotRequestsService, TimeSlotRequestsService>();
        services.AddSingleton<IStatisticsDataRequestsService, StatisticsDataRequestsService>();
        services.AddSingleton<ISettingsRequestsService, SettingsRequestsService>();
        services.AddSingleton<ITimerSettingsRequestsService, TimerSettingsRequestsService>();
    }

    private static void AddCommandsServices(this IServiceCollection services)
    {
        services.AddSingleton<IAiSettingsCommandsService, AiSettingsCommandsService>();
        services.AddSingleton<INoteCommandsService, NoteCommandsService>();
        services.AddSingleton<INoteTypeCommandsService, NoteTypeCommandsService>();
        services.AddSingleton<ISprintCommandsService, SprintCommandsService>();
        services.AddSingleton<ITicketCommandsService, TicketCommandsService>();
        services.AddSingleton<IWorkDayCommandsService, WorkDayCommandsService>();
        services.AddSingleton<ITagCommandsService, TagCommandsService>();
        services.AddSingleton<ITimeSlotCommandsService, TimeSlotCommandService>();
        services.AddSingleton<IStatisticsDataCommandsService, StatisticsDataCommandsService>();
        services.AddSingleton<ISettingsCommandsService, SettingsCommandsService>();
        services.AddSingleton<ITimerSettingsCommandsService, TimerSettingsCommandsService>();
    }

    private static void AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<SendTraceReportCommand, Unit>, SendTraceReportCommandHandler>();
        
        services.AddScoped<IRequestHandler<CreateAiSettingsCommand, Unit>, CreateAiSettingsCommandHandler>();
        services.AddScoped<IRequestHandler<ChangePromptCommand, Unit>, ChangePromptCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeLanguageModelCommand, Unit>, ChangeLanguageModelCommandHandler>();

        services.AddScoped<IRequestHandler<CreateNoteTypeCommand, Unit>, CreateNoteTypeCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeNoteTypeNameCommand, Unit>, ChangeNoteTypeNameCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeNoteTypeColorCommand, Unit>, ChangeNoteTypeColorCommandHandler>();

        services.AddScoped<IRequestHandler<CreateNoteCommand, Unit>, CreateNoteCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateNoteCommand, Unit>, UpdateNoteCommandHandler>();

        services.AddScoped<IRequestHandler<CreateTicketCommand, Unit>, CreateTicketCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateTicketDataCommand, Unit>, UpdateTicketDataCommandHandler>();
        services
            .AddScoped<IRequestHandler<UpdateTicketDocumentationCommand, Unit>,
                UpdateTicketDocumentationCommandHandler>();

        services.AddScoped<IRequestHandler<CreateSprintCommand, Unit>, CreateSprintCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateSprintDataCommand, Unit>, UpdateSprintDataCommandHandler>();
        services
            .AddScoped<IRequestHandler<AddTicketToActiveSprintCommand, Unit>, AddTicketToActiveSprintCommandHandler>();
        services.AddScoped<IRequestHandler<SetSprintActiveStatusCommand, Unit>, SetSprintActiveStatusCommandHandler>();
        services.AddScoped<IRequestHandler<AddTicketToSprintCommand, Unit>, AddTicketToSprintCommandHandler>();

        services.AddScoped<IRequestHandler<CreateStatisticsDataCommand, Unit>, CreateStatisticsDataCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeProductivityCommand, Unit>, ChangeProductivityCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeTagSelectionCommand, Unit>, ChangeTagSelectionCommandHandler>();

        services.AddScoped<IRequestHandler<UpdateTagCommand, Unit>, UpdateTagCommandHandler>();
        services.AddScoped<IRequestHandler<CreateTagCommand, Unit>, CreateTagCommandHandler>();

        services.AddScoped<IRequestHandler<CreateWorkDayCommand, Unit>, CreateWorkDayCommandHandler>();

        services.AddScoped<IRequestHandler<CreateSettingsCommand, Unit>, CreateSettingsCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateSettingsCommand, Unit>, UpdateSettingsCommandHandler>();

        services.AddScoped<IRequestHandler<CreateTimeSlotCommand, Unit>, CreateTimeSlotCommandHandler>();
        services.AddScoped<IRequestHandler<AddNoteCommand, Unit>, AddNoteCommandHandler>();
        services.AddScoped<IRequestHandler<SetEndTimeCommand, Unit>, SetEndTimeCommandHandler>();
        services.AddScoped<IRequestHandler<SetStartTimeCommand, Unit>, SetStartTimeCommandHandler>();

        services.AddScoped<IRequestHandler<CreateTimerSettingsCommand, Unit>, CreateTimerSettingsCommandHandler>();
        services
            .AddScoped<IRequestHandler<ChangeDocuTimerSaveIntervalCommand, Unit>,
                ChangeDocuTimerIntervalCommandHandler>();
        services
            .AddScoped<IRequestHandler<ChangeSnapshotSaveIntervalCommand, Unit>,
                ChangeSnapshotSaveIntervalCommandHandler>();
    }

    private static void AddCommandToEventTranslators(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsCommandsToEventTranslator, SettingsCommandsToEventTranslator>();
        services.AddSingleton<INoteCommandsToEventTranslator, NoteCommandsToEventTranslator>();
        services.AddSingleton<ISprintCommandsToEventTranslator, SprintCommandsToEventTranslator>();
        services.AddSingleton<IStatisticsDataCommandsToEventTranslator, StatisticsDataCommandsToEventTranslator>();
        services.AddSingleton<ITagCommandsToEventTranslator, TagCommandsToEventTranslator>();
        services.AddSingleton<ITicketCommandsToEventTranslator, TicketCommandsToEventTranslator>();
        services.AddSingleton<ITimeSlotCommandsToEventTranslator, TimeSlotCommandsToEventTranslator>();
        services.AddSingleton<IWorkDayCommandsToEventTranslator, WorkDayCommandsToEventTranslator>();
        services.AddSingleton<IAiSettingsCommandsToEventTranslator, AiSettingsCommandsToEventTranslator>();
        services.AddSingleton<INoteTypeCommandsToEventTranslator, NoteTypeCommandsToEventTranslator>();
        services.AddSingleton<ITimerSettingsCommandsToEventTranslator, TimerSettingsCommandsToEventTranslator>();
    }

    private static void AddCommandToNotificationTranslators(this IServiceCollection services)
    {
        services.AddSingleton<INoteCommandsToNotificationTranslator, NoteCommandsToNotificationTranslator>();
        services.AddSingleton<ISprintCommandsToNotificationTranslator, SprintCommandsToNotificationTranslator>();
        services.AddSingleton<ITagCommandsToNotificationTranslator, TagCommandsToNotificationTranslator>();
        services.AddSingleton<ITicketCommandsToNotificationTranslator, TicketCommandsToNotificationTranslator>();
        services.AddSingleton<IWorkDayCommandsToNotificationTranslator, WorkDayCommandsToNotificationTranslator>();
        services.AddSingleton<INoteTypeCommandsToNotificationTranslator, NoteTypeCommandsToNotificationTranslator>();
        services
            .AddSingleton<ITimerSettingsCommandsToNotificationTranslator,
                TimerSettingsCommandsToNotificationTranslator>();
    }

    private static void AddRequestHandlers(this IServiceCollection services)
    {
        //Replay
        services.AddScoped<IRequestHandler<GetTicketReplayByIdRequest, TicketReplayDecorator>,
            GetTicketReplayByIdRequestHandler>();

        //Note
        services.AddScoped<IRequestHandler<GetNotesByTimeSlotIdRequest, List<NoteDto>>,
            GetNotesByTimeSlotIdRequestHandler>();
        services.AddScoped<IRequestHandler<GetNotesByTicketIdRequest, List<NoteDto>>,
            GetNotesByTicketIdRequestHandler>();

        //NoteType
        services
            .AddScoped<IRequestHandler<GetAllNoteTypesRequest, List<NoteTypeDto>>,
                GetAllNoteTypesRequestHandler>();
        services
            .AddScoped<IRequestHandler<GetNoteTypeByIdRequest, NoteTypeDto?>,
                GetNoteTypeByIdRequestHandler>();

        //Sprint
        services.AddScoped<IRequestHandler<GetAllSprintsRequest, List<SprintDto>>, GetAllSprintsRequestHandler>();
        services.AddScoped<IRequestHandler<GetActiveSprintRequest, SprintDto?>, GetActiveSprintRequestHandler>();

        services
            .AddScoped<IRequestHandler<GetStatisticsDataByTimeSlotIdRequest, StatisticsDataDto>,
                GetStatisticsDataByTimeSlotIdRequestHandler>();

        //Tag
        services.AddScoped<IRequestHandler<GetTagByIdRequest, TagDto>, GetTagByIdRequestHandler>();
        services.AddScoped<IRequestHandler<GetAllTagsRequest, List<TagDto>>, GetAllTagsRequestHandler>();
        services.AddScoped<IRequestHandler<GetAllTagsRequest, List<TagDto>>, GetAllTagsRequestHandler>();

        //Ticket
        services.AddScoped<IRequestHandler<GetAllTicketsRequest, List<TicketDto>>, GetAllTicketsRequestHandler>();
        services
            .AddScoped<IRequestHandler<GetTicketsForCurrentSprintRequest, List<TicketDto>>,
                GetTicketsForCurrentSprintRequestHandler>();
        services
            .AddScoped<IRequestHandler<GetTicketsWithShowAllSwitchRequest, List<TicketDto>>,
                GetTicketsWithShowAllSwitchRequestHandler>();

        //TimeSlot
        services.AddScoped<IRequestHandler<GetTimeSlotByIdRequest, TimeSlotDto>, GetTimeSlotByIdRequestHandler>();
        services
            .AddScoped<IRequestHandler<GetTimeSlotsForWorkDayIdRequest, List<TimeSlotDto>>,
                GetTimeSlotsForWorkDayIdRequestHandler>();

        //WorkDay
        services.AddScoped<IRequestHandler<GetAllWorkDaysRequest, List<WorkDayDto>>, GetAllWorkDaysRequestHandler>();
        services.AddScoped<IRequestHandler<GetSelectedWorkDayRequest, WorkDayDto>, GetSelectedWorkDayRequestHandler>();
        services.AddScoped<IRequestHandler<GetWorkDayByDateRequest, WorkDayDto?>, GetWorkDayByDateRequestHandler>();

        //Settings
        services
            .AddScoped<IRequestHandler<GetSettingsRequest, SettingsDto?>, GetSettingsRequestHandler>();
        services.AddScoped<IRequestHandler<IsExportPathValidRequest, bool>, IsExportPathValidRequestHandler>();

        //AiSettings
        services.AddScoped<IRequestHandler<GetAiSettingsRequest, AiSettingsDto>, GetAiSettingsRequestHandler>();

        //TimerSettings
        services
            .AddScoped<IRequestHandler<GetTimerSettingsRequest, TimerSettingsDto?>, GetTimerSettingsRequestHandler>();
        services
            .AddScoped<IRequestHandler<IsTimerSettingExistingRequest, bool>, IsTimerSettingExistingRequestHandler>();
    }
}