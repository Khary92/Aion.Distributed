using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.AiSettings;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.StatisticsData;
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
using Proto.Requests.Sprints;
using Proto.Requests.StatisticsData;
using Proto.Requests.Tags;
using Proto.Requests.TicketReplay;
using Proto.Requests.Tickets;
using Proto.Requests.TimerSettings;
using Proto.Requests.TimeSlots;
using Proto.Requests.UseCase;
using Proto.Requests.WorkDays;
using Service.Proto.Shared.Requests.NoteTypes;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tags;
using Service.Proto.Shared.Requests.Tickets;

namespace Client.Desktop.Communication.Requests;

public class RequestSender(
    IAiSettingsRequestSender aiSettingsRequestSender,
    INotesRequestSender notesRequestSender,
    INoteTypeRequestSender noteTypeRequestSender,
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
    {
        return await aiSettingsRequestSender.Send(request);
    }

    public async Task<bool> Send(AiSettingExistsRequestProto request)
    {
        return await aiSettingsRequestSender.Send(request);
    }

    public async Task<List<NoteDto>> Send(GetNotesByTicketIdRequestProto request)
    {
        return await notesRequestSender.Send(request);
    }

    public async Task<List<NoteDto>> Send(GetNotesByTimeSlotIdRequestProto request)
    {
        return await notesRequestSender.Send(request);
    }

    public async Task<List<NoteTypeDto>> Send(GetAllNoteTypesRequestProto request)
    {
        var getAllNoteTypesResponseProto = await noteTypeRequestSender.Send(request);
        return getAllNoteTypesResponseProto.ToDtoList();
    }

    public async Task<NoteTypeDto> Send(GetNoteTypeByIdRequestProto request)
    {
        var noteTypeProto = await noteTypeRequestSender.Send(request);
        return noteTypeProto.ToDto();
    }
    
    public async Task<SprintDto?> Send(GetActiveSprintRequestProto request)
    {
        var sprintProto = await sprintRequestSender.Send(request);
        return sprintProto.ToDto();
    }

    public async Task<List<SprintDto?>> Send(GetAllSprintsRequestProto request)
    {
        var sprintListProto = await sprintRequestSender.Send(request);
        return sprintListProto.ToDtoList();
    }

    public async Task<StatisticsDataDto> Send(GetStatisticsDataByTimeSlotIdRequestProto request)
    {
        return await statisticsDataRequestSender.Send(request);
    }

    public async Task<List<TagDto>> Send(GetAllTagsRequestProto request)
    {
        var tagDtos = await tagRequestSender.Send(request);
        return tagDtos.ToDtoList();
    }

    public async Task<TagDto> Send(GetTagByIdRequestProto request)
    {
        var tagProto = await tagRequestSender.Send(request);
        return tagProto.ToDto();
    }

    public async Task<List<TagDto>> Send(GetTagsByIdsRequestProto request)
    {
        var tagListProto = await tagRequestSender.Send(request);
        return tagListProto.ToDtoList();
    }

    public async Task<List<TicketDto>> Send(GetAllTicketsRequestProto request)
    {
        var ticketListProto = await ticketRequestSender.Send(request);
        return ticketListProto.ToDtoList();
    }

    public async Task<List<TicketDto>> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        var ticketListProto = await ticketRequestSender.Send(request);
        return ticketListProto.ToDtoList();
    }

    public async Task<List<TicketDto>> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        var ticketListProto = await ticketRequestSender.Send(request);
        return ticketListProto.ToDtoList();
    }

    public async Task<TicketDto> Send(GetTicketByIdRequestProto request)
    {
        var ticketProto = await ticketRequestSender.Send(request);
        return ticketProto.ToDto();
    }

    public async Task<TimerSettingsDto> Send(GetTimerSettingsRequestProto request)
    {
        return await timerSettingsRequestSender.Send(request);
    }

    public async Task<bool> Send(IsTimerSettingExistingRequestProto request)
    {
        return await timerSettingsRequestSender.Send(request);
    }

    public async Task<List<TimeSlotDto>> Send(GetTimeSlotsForWorkDayIdRequestProto request)
    {
        return await timeSlotRequestSender.Send(request);
    }

    public async Task<TimeSlotDto> Send(GetTimeSlotByIdRequestProto request)
    {
        return await timeSlotRequestSender.Send(request);
    }

    public async Task<List<WorkDayDto>> Send(GetAllWorkDaysRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<WorkDayDto> Send(GetSelectedWorkDayRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<WorkDayDto> Send(GetWorkDayByDateRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<bool> Send(IsWorkDayExistingRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<List<DocumentationReplayDto>> Send(GetTicketReplaysByIdRequestProto request)
    {
        return await ticketReplayRequestSender.Send(request);
    }

    public async Task<TimeSlotControlDataListProto> Send(GetTimeSlotControlDataRequestProto request)
    {
        return await useCaseRequestSender.Send(request);
    }

    public async Task<AnalysisBySprintDecorator> Send(GetSprintAnalysisById request)
    {
        return await analysisRequestSender.Send(request);
    }

    public async Task<AnalysisByTicketDecorator> Send(GetTicketAnalysisById request)
    {
        return await analysisRequestSender.Send(request);
    }

    public async Task<AnalysisByTagDecorator> Send(GetTagAnalysisById request)
    {
        return await analysisRequestSender.Send(request);
    }
}