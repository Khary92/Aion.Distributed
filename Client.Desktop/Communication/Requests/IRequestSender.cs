using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.TimerSettings;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.UseCase;
using Client.Desktop.Communication.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests;

public interface IRequestSender :
    INotesRequestSender,
    IStatisticsDataRequestSender,
    ITimerSettingsRequestSender,
    ITimeSlotRequestSender,
    IWorkDayRequestSender,
    ITicketReplayRequestSender,
    IUseCaseRequestSender,
    IAnalysisRequestSender, 
    ISharedRequestSender;