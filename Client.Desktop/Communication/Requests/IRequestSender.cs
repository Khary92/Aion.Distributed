using Client.Desktop.Communication.Requests.AiSettings;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Notes;
using Client.Desktop.Communication.Requests.NoteTypes;
using Client.Desktop.Communication.Requests.Replays;
using Client.Desktop.Communication.Requests.Settings;
using Client.Desktop.Communication.Requests.StatisticsData;
using Client.Desktop.Communication.Requests.TimerSettings;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.UseCase;
using Client.Desktop.Communication.Requests.WorkDays;
using Service.Proto.Shared.Requests.Sprints;
using Service.Proto.Shared.Requests.Tags;

namespace Client.Desktop.Communication.Requests;

public interface IRequestSender :
    IAiSettingsRequestSender,
    INotesRequestSender,
    INoteTypesRequestSender,
    ISettingsRequestSender,
    IStatisticsDataRequestSender,
    ITimerSettingsRequestSender,
    ITimeSlotRequestSender,
    IWorkDayRequestSender,
    ITicketReplayRequestSender,
    IUseCaseRequestSender,
    IAnalysisRequestSender, 
    ISharedRequestSender;