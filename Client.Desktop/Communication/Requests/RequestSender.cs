using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.UseCase;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Replays;
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
using Service.Proto.Shared.Requests.TimerSettings;

namespace Client.Desktop.Communication.Requests;

public class RequestSender(
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
    public async Task<List<NoteClientModel>> Send(GetNotesByTicketIdRequestProto request)
    {
        return await notesRequestSender.Send(request);
    }

    public async Task<List<NoteClientModel>> Send(GetNotesByTimeSlotIdRequestProto request)
    {
        return await notesRequestSender.Send(request);
    }

    public async Task<List<NoteTypeClientModel>> Send(GetAllNoteTypesRequestProto request)
    {
        var getAllNoteTypesResponseProto = await noteTypeRequestSender.Send(request);
        return getAllNoteTypesResponseProto.ToModelList();
    }

    public async Task<NoteTypeClientModel> Send(GetNoteTypeByIdRequestProto request)
    {
        var noteTypeProto = await noteTypeRequestSender.Send(request);
        return noteTypeProto.ToModel();
    }

    public async Task<SprintClientModel?> Send(GetActiveSprintRequestProto request)
    {
        // TODO make this nullable!
        var sprintProto = await sprintRequestSender.Send(request);
        return sprintProto?.ToModel();
    }

    public async Task<List<SprintClientModel?>> Send(GetAllSprintsRequestProto request)
    {
        var sprintListProto = await sprintRequestSender.Send(request);
        return sprintListProto.ToModelList();
    }

    public async Task<StatisticsDataClientModel> Send(GetStatisticsDataByTimeSlotIdRequestProto request)
    {
        return await statisticsDataRequestSender.Send(request);
    }

    public async Task<List<TagClientModel>> Send(GetAllTagsRequestProto request)
    {
        var tagDtos = await tagRequestSender.Send(request);
        return tagDtos.ToModelList();
    }

    public async Task<TagClientModel> Send(GetTagByIdRequestProto request)
    {
        var tagProto = await tagRequestSender.Send(request);
        return tagProto.ToModel();
    }

    public async Task<List<TagClientModel>> Send(GetTagsByIdsRequestProto request)
    {
        var tagListProto = await tagRequestSender.Send(request);
        return tagListProto.ToModelList();
    }

    public async Task<List<TicketClientModel>> Send(GetAllTicketsRequestProto request)
    {
        var ticketListProto = await ticketRequestSender.Send(request);
        return ticketListProto.ToModelList();
    }

    public async Task<List<TicketClientModel>> Send(GetTicketsForCurrentSprintRequestProto request)
    {
        var ticketListProto = await ticketRequestSender.Send(request);
        return ticketListProto.ToModelList();
    }

    public async Task<List<TicketClientModel>> Send(GetTicketsWithShowAllSwitchRequestProto request)
    {
        var ticketListProto = await ticketRequestSender.Send(request);
        return ticketListProto.ToModelList();
    }

    public async Task<TicketClientModel> Send(GetTicketByIdRequestProto request)
    {
        var ticketProto = await ticketRequestSender.Send(request);
        return ticketProto.ToModel();
    } 
    
    public async Task<List<TimeSlotClientModel>> Send(GetTimeSlotsForWorkDayIdRequestProto request)
    {
        return await timeSlotRequestSender.Send(request);
    }

    public async Task<TimeSlotClientModel> Send(GetTimeSlotByIdRequestProto request)
    {
        return await timeSlotRequestSender.Send(request);
    }

    public async Task<List<WorkDayClientModel>> Send(GetAllWorkDaysRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<WorkDayClientModel> Send(GetSelectedWorkDayRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<WorkDayClientModel> Send(GetWorkDayByDateRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<bool> Send(IsWorkDayExistingRequestProto request)
    {
        return await workDayRequestSender.Send(request);
    }

    public async Task<List<DocumentationReplay>> Send(GetTicketReplaysByIdRequestProto request)
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