using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications;
using Client.Desktop.Communication.Policies;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Notes.Records;
using Client.Desktop.Communication.Requests.NoteType;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.Replays.Records;
using Client.Desktop.Communication.Requests.Sprint;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.StatisticsData.Records;
using Client.Desktop.Communication.Requests.Tag;
using Client.Desktop.Communication.Requests.Ticket;
using Client.Desktop.Communication.Requests.Timer;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.TimeSlots.Records;
using Client.Desktop.Communication.Requests.UseCase;
using Client.Desktop.Communication.Requests.UseCase.Records;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Replays;
using Proto.Requests.TimerSettings;
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
    ITimeSlotRequestSender timeSlotRequestSender,
    IWorkDayRequestSender workDayRequestSender,
    ITicketReplayRequestSender ticketReplayRequestSender,
    IUseCaseRequestSender useCaseRequestSender,
    IAnalysisRequestSender analysisRequestSender,
    ITimerSettingsRequestSender timerSettingsRequestSender,
    RequestRetryPolicy requestSender) : IRequestSender
{
    public async Task<List<NoteClientModel>> Send(ClientGetNotesByTicketIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            notesRequestSender.Send(request));
    }

    public async Task<List<NoteClientModel>> Send(ClientGetNotesByTimeSlotIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            notesRequestSender.Send(request));
    }

    public async Task<List<NoteTypeClientModel>> Send(ClientGetAllNoteTypesRequest request)
    {
        var getAllNoteTypesResponse = await requestSender.Policy.ExecuteAsync(() =>
            noteTypeRequestSender.Send(request.ToProto()));

        return getAllNoteTypesResponse.ToClientModelList();
    }

    public async Task<NoteTypeClientModel> Send(ClientGetNoteTypeByIdRequest request)
    {
        var noteType = await requestSender.Policy.ExecuteAsync(() =>
            noteTypeRequestSender.Send(request.ToProto()));

        return noteType.ToClientModel();
    }

    public async Task<TimerSettingsClientModel> Send(ClientGetTimerSettingsRequest request)
    {
        var timerSettingsProto = await requestSender.Policy.ExecuteAsync(() =>
            timerSettingsRequestSender.Send(request.ToProto()));

        return timerSettingsProto.ToClientModel();
    }

    public async Task<SprintClientModel?> Send(ClientGetActiveSprintRequest request)
    {
        // TODO make this nullable!
        var sprint = await requestSender.Policy.ExecuteAsync(() =>
            sprintRequestSender.Send(request.ToProto()));

        return sprint?.ToClientModel();
    }

    public async Task<List<SprintClientModel?>> Send(ClientGetAllSprintsRequest request)
    {
        var sprintList = await requestSender.Policy.ExecuteAsync(() =>
            sprintRequestSender.Send(request.ToProto()));

        return sprintList.ToClientModelList();
    }

    public async Task<StatisticsDataClientModel> Send(ClientGetStatisticsDataByTimeSlotIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            statisticsDataRequestSender.Send(request));
    }

    public async Task<List<TagClientModel>> Send(ClientGetAllTagsRequest request)
    {
        var tagDtos = await requestSender.Policy.ExecuteAsync(() =>
            tagRequestSender.Send(request.ToProto()));

        return tagDtos.ToClientModelList();
    }

    public async Task<TagClientModel> Send(ClientGetTagByIdRequest request)
    {
        var tag = await requestSender.Policy.ExecuteAsync(() =>
            tagRequestSender.Send(request.ToProto()));

        return tag.ToClientModel();
    }

    public async Task<List<TagClientModel>> Send(ClientGetTagsByIdsRequest request)
    {
        var tagList = await requestSender.Policy.ExecuteAsync(() =>
            tagRequestSender.Send(request.ToProto()));

        return tagList.ToClientModelList();
    }

    public async Task<List<TicketClientModel>> Send(ClientGetAllTicketsRequest request)
    {
        var ticketList = await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));

        return ticketList.ToClientModelList();
    }

    public async Task<List<TicketClientModel>> Send(ClientGetTicketsForCurrentSprintRequest request)
    {
        var ticketList = await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));

        return ticketList.ToClientModelList();
    }

    public async Task<List<TicketClientModel>> Send(ClientGetTicketsWithShowAllSwitchRequest request)
    {
        var ticketList = await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));

        return ticketList.ToClientModelList();
    }

    public async Task<TicketClientModel> Send(ClientGetTicketByIdRequest request)
    {
        var ticket = await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));

        return ticket.ToClientModel();
    }

    public async Task<List<TimeSlotClientModel>> Send(ClientGetTimeSlotsForWorkDayIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            timeSlotRequestSender.Send(request));
    }

    public async Task<TimeSlotClientModel> Send(ClientGetTimeSlotByIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            timeSlotRequestSender.Send(request));
    }

    public async Task<List<WorkDayClientModel>> Send(ClientGetAllWorkDaysRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            workDayRequestSender.Send(request));
    }

    public async Task<WorkDayClientModel> Send(ClientGetSelectedWorkDayRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            workDayRequestSender.Send(request));
    }

    public async Task<WorkDayClientModel> Send(ClientGetWorkDayByDateRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            workDayRequestSender.Send(request));
    }

    public async Task<bool> Send(ClientIsWorkDayExistingRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            workDayRequestSender.Send(request));
    }

    public async Task<List<DocumentationReplay>> Send(ClientGetTicketReplaysByIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            ticketReplayRequestSender.Send(request));
    }

    public async Task<List<ClientGetTimeSlotControlResponse>> Send(ClientGetTimeSlotControlDataRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            useCaseRequestSender.Send(request));
    }

    public async Task<AnalysisBySprintDecorator> Send(ClientGetSprintAnalysisById request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            analysisRequestSender.Send(request));
    }

    public async Task<AnalysisByTicketDecorator> Send(ClientGetTicketAnalysisById request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            analysisRequestSender.Send(request));
    }

    public async Task<AnalysisByTagDecorator> Send(ClientGetTagAnalysisById request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            analysisRequestSender.Send(request));
    }
}