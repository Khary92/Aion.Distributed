using Client.Desktop.Communication.Commands.AiSettings;
using Client.Desktop.Communication.Commands.Notes;
using Client.Desktop.Communication.Commands.NoteTypes;
using Client.Desktop.Communication.Commands.Settings;
using Client.Desktop.Communication.Commands.StatisticsData;
using Client.Desktop.Communication.Commands.Tags;
using Client.Desktop.Communication.Commands.TimerSettings;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.UseCases;
using Client.Desktop.Communication.Commands.WorkDays;
using Service.Proto.Shared.Commands.Sprints;
using Service.Proto.Shared.Commands.Tickets;

namespace Client.Desktop.Communication.Commands;

public interface ICommandSender :
    IAiSettingsCommandSender,
    INoteCommandSender,
    INoteTypeCommandSender,
    ISettingsCommandSender,
    ISprintCommandSender,
    IStatisticsDataCommandSender,
    ITagCommandSender,
    ITimerSettingsCommandSender,
    ITimeSlotCommandSender,
    IUseCaseCommandSender,
    IWorkDayCommandSender,
    ITicketCommandSender;