using Client.Avalonia.Communication.Requests.AiSettings;
using Client.Avalonia.Communication.Requests.Notes;
using Client.Avalonia.Communication.Requests.NoteTypes;
using Client.Avalonia.Communication.Requests.Settings;
using Client.Avalonia.Communication.Requests.Sprints;
using Client.Avalonia.Communication.Requests.StatisticsData;
using Client.Avalonia.Communication.Requests.Tags;
using Client.Avalonia.Communication.Requests.Tickets;
using Client.Avalonia.Communication.Requests.TimerSettings;
using Client.Avalonia.Communication.Requests.TimeSlots;
using Client.Avalonia.Communication.Requests.WorkDays;

namespace Client.Avalonia.Communication.Requests;

public interface IRequestSender :
    IAiSettingsRequestSender,
    INotesRequestSender,
    INoteTypesRequestSender,
    ISettingsRequestSender,
    ISprintRequestSender,
    IStatisticsDataRequestSender,
    ITagRequestSender,
    ITicketRequestSender,
    ITimerSettingsRequestSender,
    ITimeSlotRequestSender,
    IWorkDayRequestSender;