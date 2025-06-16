using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.AiSettings;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.NoteTypes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.Settings;
using Client.Desktop.Communication.Requests.Sprints;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.Tags;
using Client.Desktop.Communication.Requests.Tickets;
using Client.Desktop.Communication.Requests.TimerSettings;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.UseCase;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.Decorators;
using Client.Desktop.DTO;
using Client.Desktop.Replays;
using Proto.Requests.AiSettings;
using Proto.Requests.AnalysisData;
using Proto.Requests.Notes;
using Proto.Requests.NoteTypes;
using Proto.Requests.Settings;
using Proto.Requests.Sprints;
using Proto.Requests.StatisticsData;
using Proto.Requests.Tags;
using Proto.Requests.TicketReplay;
using Proto.Requests.Tickets;
using Proto.Requests.TimerSettings;
using Proto.Requests.TimeSlots;
using Proto.Requests.UseCase;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests;

public class RequestSender(
    IAiSettingsRequestSender aiSettingsRequestSender,
    INotesRequestSender notesRequestSender,
    INoteTypesRequestSender noteTypesRequestSender,
    ISettingsRequestSender settingsRequestSender,
    ISprintRequestSender sprintRequestSender,
    IStatisticsDataRequestSender statisticsDataRequestSender,
    ITagRequestSender tagRequestSender,
    ITicketRequestSender ticketRequestSender,
    ITimerSettingsRequestSender timerSettingsRequestSender,
    ITimeSlotRequestSender timeSlotRequestSender,
    IWorkDayRequestSender workDayRequestSender,
    ITicketReplayRequestSender ticketReplayRequestSender,
    IUseCaseRequestSender useCaseRequestSender,
    IAnalysisRequestSender analysisRequestSender) : IRequestSender
{
    public async Task<AiSettingsDto> Send(GetAiSettingsRequestProto request)
        => await aiSettingsRequestSender.Send(request);

    public async Task<bool> Send(AiSettingExistsRequestProto request)
        => await aiSettingsRequestSender.Send(request);

    public async Task<List<NoteDto>> Send(GetNotesByTicketIdRequestProto request)
        => await notesRequestSender.Send(request);

    public async Task<List<NoteDto>> Send(GetNotesByTimeSlotIdRequestProto request)
        => await notesRequestSender.Send(request);

    public async Task<List<NoteTypeDto>> Send(GetAllNoteTypesRequestProto request)
        => await noteTypesRequestSender.Send(request);

    public async Task<NoteTypeDto> Send(GetNoteTypeByIdRequestProto request)
        => await noteTypesRequestSender.Send(request);

    public async Task<SettingsDto> Send(GetSettingsRequestProto request)
        => await settingsRequestSender.Send(request);

    public async Task<bool> Send(IsExportPathValidRequestProto request)
        => await settingsRequestSender.Send(request);

    public async Task<bool> Send(SettingsExistsRequestProto request)
        => await settingsRequestSender.Send(request);

    public async Task<SprintDto?> Send(GetActiveSprintRequestProto request)
        => await sprintRequestSender.Send(request);

    public async Task<List<SprintDto>> Send(GetAllSprintsRequestProto request)
        => await sprintRequestSender.Send(request);

    public async Task<StatisticsDataDto> Send(GetStatisticsDataByTimeSlotIdRequestProto request)
        => await statisticsDataRequestSender.Send(request);

    public async Task<List<TagDto>> Send(GetAllTagsRequestProto request)
        => await tagRequestSender.Send(request);

    public async Task<TagDto> Send(GetTagByIdRequestProto request)
        => await tagRequestSender.Send(request);

    public async Task<List<TagDto>> Send(GetTagsByIdsRequestProto request)
        => await tagRequestSender.Send(request);

    public async Task<List<TicketDto>> Send(GetAllTicketsRequestProto request)
        => await ticketRequestSender.Send(request);

    public async Task<List<TicketDto>> Send(GetTicketsForCurrentSprintRequestProto request)
        => await ticketRequestSender.Send(request);

    public async Task<List<TicketDto>> Send(GetTicketsWithShowAllSwitchRequestProto request)
        => await ticketRequestSender.Send(request);

    public async Task<TicketDto> Send(GetTicketByIdRequestProto request)
        => await ticketRequestSender.Send(request);

    public async Task<TimerSettingsDto> Send(GetTimerSettingsRequestProto request)
        => await timerSettingsRequestSender.Send(request);

    public async Task<bool> Send(IsTimerSettingExistingRequestProto request)
        => await timerSettingsRequestSender.Send(request);

    public async Task<List<TimeSlotDto>> Send(GetTimeSlotsForWorkDayIdRequestProto request)
        => await timeSlotRequestSender.Send(request);

    public async Task<TimeSlotDto> Send(GetTimeSlotByIdRequestProto request)
        => await timeSlotRequestSender.Send(request);

    public async Task<List<WorkDayDto>> Send(GetAllWorkDaysRequestProto request)
        => await workDayRequestSender.Send(request);

    public async Task<WorkDayDto> Send(GetSelectedWorkDayRequestProto request)
        => await workDayRequestSender.Send(request);

    public async Task<WorkDayDto> Send(GetWorkDayByDateRequestProto request)
        => await workDayRequestSender.Send(request);

    public async Task<List<DocumentationReplayDto>> Send(GetTicketReplaysByIdRequestProto request)
        => await ticketReplayRequestSender.Send(request);

    public async Task<TimeSlotControlDataProto> Send(GetTimeSlotControlDataRequestProto request)
        => await useCaseRequestSender.Send(request);

    public async Task<AnalysisBySprintDecorator> Send(GetSprintAnalysisById request)
        => await analysisRequestSender.Send(request);

    public async Task<AnalysisByTicketDecorator> Send(GetTicketAnalysisById request)
        => await analysisRequestSender.Send(request);

    public async Task<AnalysisByTagDecorator> Send(GetTagAnalysisById request)
        => await analysisRequestSender.Send(request);
}