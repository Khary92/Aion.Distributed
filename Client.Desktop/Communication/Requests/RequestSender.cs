using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Policies;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.Communication.Requests.Client;
using Client.Desktop.Communication.Requests.Client.Records;
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
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Replays;
using INoteTypeRequestSender = Client.Desktop.Communication.Requests.NoteType.INoteTypeRequestSender;
using ISprintRequestSender = Client.Desktop.Communication.Requests.Sprint.ISprintRequestSender;
using ITagRequestSender = Client.Desktop.Communication.Requests.Tag.ITagRequestSender;
using ITicketRequestSender = Client.Desktop.Communication.Requests.Ticket.ITicketRequestSender;
using ITimerSettingsRequestSender = Client.Desktop.Communication.Requests.Timer.ITimerSettingsRequestSender;

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
    IClientRequestSender useCaseRequestSender,
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
        return await requestSender.Policy.ExecuteAsync(() =>
            noteTypeRequestSender.Send(request.ToProto()));
    }

    public async Task<NoteTypeClientModel> Send(ClientGetNoteTypeByIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            noteTypeRequestSender.Send(request.ToProto()));
    }

    public async Task<TimerSettingsClientModel> Send(ClientGetTimerSettingsRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            timerSettingsRequestSender.Send(request.ToProto()));
    }

    public async Task<SprintClientModel?> Send(ClientGetActiveSprintRequest request)
    {
        // TODO make this nullable!
        return await requestSender.Policy.ExecuteAsync(() =>
            sprintRequestSender.Send(request.ToProto()));
    }

    public async Task<List<SprintClientModel?>> Send(ClientGetAllSprintsRequest request)
    {
        // TODO check warning
        return await requestSender.Policy.ExecuteAsync(() =>
            sprintRequestSender.Send(request.ToProto()));
    }

    public async Task<StatisticsDataClientModel> Send(ClientGetStatisticsDataByTimeSlotIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            statisticsDataRequestSender.Send(request));
    }

    public async Task<List<TagClientModel>> Send(ClientGetAllTagsRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            tagRequestSender.Send(request.ToProto()));
    }

    public async Task<TagClientModel> Send(ClientGetTagByIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            tagRequestSender.Send(request.ToProto()));
    }

    public async Task<List<TagClientModel>> Send(ClientGetTagsByIdsRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            tagRequestSender.Send(request.ToProto()));
    }

    public async Task<List<TicketClientModel>> Send(ClientGetAllTicketsRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));
    }

    public async Task<List<TicketClientModel>> Send(ClientGetTicketsForCurrentSprintRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));
    }

    public async Task<List<TicketClientModel>> Send(ClientGetTicketsWithShowAllSwitchRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));
    }

    public async Task<TicketClientModel> Send(ClientGetTicketByIdRequest request)
    {
        return await requestSender.Policy.ExecuteAsync(() =>
            ticketRequestSender.Send(request.ToProto()));
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

    public async Task<List<ClientGetTrackingControlResponse>> Send(ClientGetTrackingControlDataRequest request)
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