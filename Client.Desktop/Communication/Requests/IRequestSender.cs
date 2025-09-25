using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Client;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests;

public interface IRequestSender :
    INotesRequestSender,
    IStatisticsDataRequestSender,
    ITimeSlotRequestSender,
    IWorkDayRequestSender,
    ITicketReplayRequestSender,
    IClientRequestSender,
    IAnalysisRequestSender,
    ISharedRequestSender;