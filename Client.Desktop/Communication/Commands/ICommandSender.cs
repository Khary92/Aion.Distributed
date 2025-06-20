using Client.Desktop.Communication.Commands.AiSettings;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.NoteTypes;
using Client.Desktop.Communication.Commands.Settings;
using Client.Desktop.Communication.Commands.Sprints;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.Tags;
using Client.Desktop.Communication.Commands.Tickets;
using Client.Desktop.Communication.Commands.TimerSettings;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TraceReports;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.WorkDays;

namespace Client.Desktop.Communication.Commands;

public interface ICommandSender :
    IAiSettingsCommandSender,
    INoteCommandSender,
    INoteTypeCommandSender,
    ISettingsCommandSender,
    ISprintCommandSender,
    IStatisticsDataCommandSender,
    ITagCommandSender,
    ITicketCommandSender,
    ITimerSettingsCommandSender,
    ITimeSlotCommandSender,
    ITraceReportCommandSender,
    IUseCaseCommandSender,
    IWorkDayCommandSender;